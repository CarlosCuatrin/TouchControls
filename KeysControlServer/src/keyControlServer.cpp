#include <microhttpd.h>
#include <windows.h>
#include <stdlib.h> 
#include <string.h>
#include <stdio.h>

#define INPUT_URL        "/input/"
#define INPUT_URL_LEN    (sizeof(INPUT_URL) - 1)
#define CHAR3(a, b, c)   ((a << 16) | (b << 8) | c)


struct Responses {
	MHD_Response *client;
	MHD_Response *input;
};


typedef struct myStruct{
	INPUT input;
	int msDelay;
}MYSTRUC, *PMYSTRUCT;


DWORD WINAPI ThreadSendInputDelayed(void *data){
	
	OutputDebugStringA("Thread Started");	
	INPUT inputData = ((PMYSTRUCT) data)->input;
	int msDelay = ((PMYSTRUCT) data)->msDelay;

	Sleep(msDelay);
	OutputDebugStringA("\nSleep 3000ms worked\n");

	//press key
	SendInput(1, &inputData, sizeof(INPUT));

	return(0);

}

void SendInputDelayed(INPUT input /*use pointer reference*/, int msDelay){
	
	//USING INPUT + ms
	PMYSTRUCT data = (PMYSTRUCT) HeapAlloc(GetProcessHeap(),HEAP_ZERO_MEMORY, sizeof(MYSTRUC) );
	if(data == NULL){
		ExitProcess(2);
	}

	(*data).input = input;
	(*data).msDelay = msDelay;

	//Create thread
	HANDLE hThreadSendInputDelayed = CreateThread(NULL, 0, ThreadSendInputDelayed, data, 0, NULL);

	//TODO: Free memory data once here, when thead has finished.
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
static int request_handler(void *cls, MHD_Connection *connection, const char *url,
	const char *method, const char *version, const char *upload_data,
	size_t *upload_data_size, void **ptr)
{
	static int dummy;

	if (0 != strcmp(method, "GET"))
		return MHD_NO;

	if (&dummy != *ptr)
	{
		*ptr = &dummy;
		return MHD_YES;
	}

	if (0 != *upload_data_size)
		return MHD_NO;

	*ptr = 0;

	if (0 == strcmp(url, "/"))
	{
		return MHD_queue_response(connection, MHD_HTTP_OK, ((Responses*)cls)->client);
	}
	else if (0 == strncmp(url, INPUT_URL, INPUT_URL_LEN))
	{
		url += INPUT_URL_LEN;
		int len = strlen(url);

		if (len < 3)
			return MHD_NO;

		switch (CHAR3(url[0], url[1], url[2]))
		{
			// mouse move
			case CHAR3('m','m','/'):
			{
				int dx, dy;

				if (2 != sscanf_s(url + 3, "%d,%d", &dx, &dy))
					return MHD_NO;

				INPUT input;
				input.type = INPUT_MOUSE;
				input.mi.dx = dx;
				input.mi.dy = dy;
				input.mi.mouseData = 0;
				input.mi.dwFlags = MOUSEEVENTF_MOVE;
				input.mi.time = 0;
				input.mi.dwExtraInfo = GetMessageExtraInfo();

				SendInput(1, &input, sizeof(INPUT));
			}
			break;

			// mouse down/up/click
			case CHAR3('m','d','/'):
			case CHAR3('m','u','/'):
			case CHAR3('m','c','/'):
			{
				unsigned int button;

				if (1 != sscanf_s(url + 3, "%u", &button) || button > 2)
					return MHD_NO;

				const DWORD options[2][3] = {
					{ MOUSEEVENTF_LEFTDOWN, MOUSEEVENTF_RIGHTDOWN, MOUSEEVENTF_MIDDLEDOWN },
					{ MOUSEEVENTF_LEFTUP,   MOUSEEVENTF_RIGHTUP,   MOUSEEVENTF_MIDDLEUP   }
				};
				
				//Struct that SendInput Needs	
				INPUT input;
				input.type = INPUT_MOUSE;
				input.mi.dx = 0;
				input.mi.dy = 0;
				input.mi.mouseData = 0;
				input.mi.dwFlags = options[url[1] == 'u'][button];
				input.mi.time = 0;
				input.mi.dwExtraInfo = GetMessageExtraInfo();

				if (url[1] == 'c')
				{
					input.mi.dwFlags = options[0][button];
					SendInput(1, &input, sizeof(INPUT));

					input.mi.dwFlags = options[1][button];
					SendInput(1, &input, sizeof(INPUT));
				}
				else
					SendInput(1, &input, sizeof(INPUT));
			}
			break;

			// key down/up/press
			case CHAR3('k','d','/'):
			//pass through
			case CHAR3('k','u','/'):
			//pass through
			case CHAR3('k','p','/'):
			{
				unsigned int key;
				unsigned int msDelay;
				
				if (1 != sscanf_s(url + 3, "%u", &key) || 0 == (key = MapVirtualKey(key, MAPVK_VK_TO_VSC)))
					return MHD_NO;
				
				//Struct that SendInput Needs
				INPUT input;
				input.type = INPUT_KEYBOARD;
				input.ki.wVk = 0;
				input.ki.wScan = key;
				input.ki.dwFlags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP * (url[1] == 'u'); //KEYEVENTF_SCANCODE = key down, KEYEVENTF_KEYUP =  key up
				input.ki.time = 0;
				input.ki.dwExtraInfo = GetMessageExtraInfo();
				
				int scan = sscanf_s(url + 3, "%u +%u", &key, &msDelay);
				//If there no msDalay data
				if( scan == 1 ){

					if (url[1] == 'p')
					{
						input.ki.dwFlags = KEYEVENTF_SCANCODE;
						SendInput(1, &input, sizeof(INPUT));

						input.ki.dwFlags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP;
						SendInput(1, &input, sizeof(INPUT));
					}
					else
						SendInput(1, &input, sizeof(INPUT));
				}
				//IF there is msDealay data
				else if( scan == 2 ){
					
					if (url[1] == 'p')
					{
						input.ki.dwFlags = KEYEVENTF_SCANCODE;
						SendInputDelayed(input, msDelay);

						input.ki.dwFlags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP;
						msDelay += 60; //make a delay so it's safe that this second thread will be executed after the first thread, and not the other way around.
						SendInputDelayed(input, msDelay);
					}
					else{
						SendInputDelayed(input, msDelay);
					}
					//make the same as above but with msDelay sent to a new thread
					//be aware of creating multiple threads... maybe creating a global flag
				}
				else{
					return MHD_NO;
				}
	
			}
			break;

			default: return MHD_NO;
		}

		return MHD_queue_response(connection, MHD_HTTP_OK, ((Responses*)cls)->input);
	}

	return MHD_NO;
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
unsigned char *read_file(const char *filename, long *size)
{
	FILE *file = fopen(filename, "rb");
	unsigned char *data = 0;

	if (file)
	{
		fseek(file, 0, SEEK_END);
		*size = ftell(file);
		data = (unsigned char*)malloc(*size);
		fseek(file, 0, SEEK_SET);
		fread(data, 1, *size, file);
		fclose(file);
	}

	return data;
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
int main(int argc, char **argv)
{
	// Sleep(2000);
	// INPUT input;
	// 	input.type = INPUT_KEYBOARD;
	// 	input.ki.wVk = 0;
	// 	input.ki.wScan = MapVirtualKey('X', MAPVK_VK_TO_VSC);
	// 	input.ki.time = 0;
	// 	input.ki.dwExtraInfo = GetMessageExtraInfo();
	// 	input.ki.dwFlags = KEYEVENTF_SCANCODE;
		
	// SendInputDelayed(input, 3000);
	// Sleep(60000);

	/*if (argc != 2)
	{
		printf("%s PORT\n", argv[0]);
		return 1;
	}*/

	char nullstr[] = "";
	long size = 0;
	unsigned char *client_data = read_file("client.html", &size);

	if (client_data == 0)
		client_data = (unsigned char*)nullstr;

	Responses response;
	response.input = MHD_create_response_from_buffer(0, nullstr, MHD_RESPMEM_PERSISTENT);
	response.client = MHD_create_response_from_buffer(size, client_data, MHD_RESPMEM_PERSISTENT);
	MHD_add_response_header(response.input, MHD_HTTP_HEADER_CONTENT_TYPE, "text/html");
	MHD_add_response_header(response.client, MHD_HTTP_HEADER_CONTENT_TYPE, "text/html");

	MHD_Daemon *daemon = MHD_start_daemon(MHD_USE_SELECT_INTERNALLY, /*atoi(argv[1])*/81,
		NULL, NULL, &request_handler, &response, MHD_OPTION_END);

	getc(stdin); //wait for a key to get pressed to finish the process

	MHD_destroy_response(response.input);
	MHD_destroy_response(response.client);
	MHD_stop_daemon(daemon);

	free(client_data);

	return 0;
}
