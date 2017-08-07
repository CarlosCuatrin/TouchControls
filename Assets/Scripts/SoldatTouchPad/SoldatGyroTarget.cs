using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoldatGyroTarget : MonoBehaviour {

	public int gyroBiasMaxSamples = 1000;		//number of measurements of gyroscope raw data (each frame, 1 measurement)
	public Image calibrateButton;
	public Slider progressBar;
	public float gyroSensitivityX;
	public float gyroSensitivityY;
	public InputField inputFieldGyroX;
	public InputField inputFieldGyroY;
	public SoldatInputClient inputControl;
	float number;

	private int gyroBiasSamplesMade;		//number of measurements of gyroscope raw data (each frame, 1 measurement)
	private Vector3 gyroBias;					//avarage bias of raw data from gyroscope
	private Color flashingTargetColor;

	void Start () {

		// Activate the gyroscope
		if(!Input.gyro.enabled)
			Input.gyro.enabled = true;

		//Set inputboxes text to vurrent gyroBias data.
		inputFieldGyroX.text =  gyroSensitivityX.ToString();
		inputFieldGyroY.text =  gyroSensitivityY.ToString();

		//Check if calibration data is alredy in memory
		if(PlayerPrefs.HasKey("gyroBiasX")&&PlayerPrefs.HasKey("gyroBiasY")){
			gyroBiasSamplesMade = gyroBiasMaxSamples + 1;
			gyroBias.x = PlayerPrefs.GetFloat("gyroBiasX");
			gyroBias.y = PlayerPrefs.GetFloat("gyroBiasY");
			//show on screen all is ok.
		}
	
	}








	void Update () {
		//SI SE APRIETA BOTON CALIBRAR arranca un contador de 2 segundos para dejar inmovil y luego RESETEA CONTADOR Y GYROBIAS y arranca calibracion.
		if(CalibrateGyro()) return; 

		StartCoroutine( inputControl.MouseMove( (int)((Input.gyro.rotationRateUnbiased.y - gyroBias.y) * gyroSensitivityY), (int)((Input.gyro.rotationRateUnbiased.x - gyroBias.x) * gyroSensitivityX) ) );

	}











	/// <summary>
	/// Calibrates gyro storing gyro data from multiple frames and calculating and avarage gyro bias.
	///returns<c>true</c>, if gyro still calibrating, <c>false</c> if gyro ends calibration.
	/// </summary>
	/// <returns><c>true</c>, if gyro still calibrating, <c>false</c> if gyro ends calibration.</returns>
	public void StartCalibration(){
		gyroBiasSamplesMade = 0;
	}

	bool CalibrateGyro(){
		if(gyroBiasSamplesMade > gyroBiasMaxSamples)
			return false;
		if(gyroBiasSamplesMade == 0){
			gyroBias = new Vector3();
		}
		gyroBiasSamplesMade++;
		gyroBias = new Vector3 (gyroBias.x + Input.gyro.rotationRateUnbiased.x,
							gyroBias.y + Input.gyro.rotationRateUnbiased.y,
			                gyroBias.z + Input.gyro.rotationRateUnbiased.z);

		//Animating Button Color flashing	
		calibrateButton.color = Color.Lerp(Color.black, Color.red, Mathf.PingPong(Time.time, 1));
		progressBar.gameObject.SetActive(true);
		progressBar.value = (float)gyroBiasSamplesMade / (float)gyroBiasMaxSamples;

		if(gyroBiasSamplesMade == gyroBiasMaxSamples){
			
			gyroBias = gyroBias / (float) gyroBiasSamplesMade;
			gyroBiasSamplesMade++;

			PlayerPrefs.SetFloat ("gyroBiasX", gyroBias.x);
			PlayerPrefs.SetFloat ("gyroBiasY", gyroBias.y);
			PlayerPrefs.Save ();

			if (PlayerPrefs.HasKey ("gyroBiasX") && PlayerPrefs.HasKey ("gyroBiasY") && PlayerPrefs.GetFloat ("gyroBiasX") == gyroBias.x) {
				Vibration.Vibrate (500);
				//eliminate warning
				calibrateButton.color = Color.black;
				progressBar.gameObject.SetActive(false);
				return false;
			}
			else{
				
				//warning signal
				calibrateButton.color = Color.red; 
				progressBar.gameObject.SetActive(false);
				return false;

			}
		}
		return true;
	}

	public void ChangeGyroSensitivityX(){
		gyroSensitivityX = float.TryParse( inputFieldGyroX.text.ToString(), out number) ? number : gyroSensitivityX;
	}
	public void ChangeGyroSensitivityY(){
		gyroSensitivityY = float.TryParse( inputFieldGyroY.text.ToString(), out number) ? number : gyroSensitivityY;
	}
}
