using UnityEngine;
using System.Collections;

public class InputActionToServer : MonoBehaviour {

    public SoldatInputClient inputClient; 

    public bool debugLogWhenDataSent = true;

    void OnEnable()
    {
        GenericTouchEvent.Jump += Jump;
        GenericTouchEvent.RunLeft += RunLeft;
        GenericTouchEvent.RunRight += RunRight;
        GenericTouchEvent.Fire += Fire;
        GenericTouchEvent.ChangeWeapon += ChangeWeapon;
        GenericTouchEvent.Hook += Hook;
       
    }

    void OnDisable()
    {
        GenericTouchEvent.Jump -= Jump;
        GenericTouchEvent.RunLeft -= RunLeft;
        GenericTouchEvent.RunRight -= RunRight;
        GenericTouchEvent.Fire -= Fire;
        GenericTouchEvent.ChangeWeapon -= ChangeWeapon;
        GenericTouchEvent.Hook -= Hook;
    }

    void Jump(object sender, InputActionEventArgs args)
    {
        if ( args != null && args.Activation)
        {
            inputClient.KeyDown(SoldatInputClient.Key.Space);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent) Debug.Log("Jump On");
            #endif
        }
        else
        {
            inputClient.KeyUp(SoldatInputClient.Key.Space);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Jump Off");
            #endif
        }

    }

    void RunLeft(object sender, InputActionEventArgs args)
    {
        if ( args != null && args.Activation)
        {
            inputClient.KeyDown(SoldatInputClient.Key.A);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Left On");
            #endif
        }
        else
        {
            inputClient.KeyUp(SoldatInputClient.Key.A);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Left Off");
            #endif
        }
        
    }
    void RunRight(object sender, InputActionEventArgs args)
    {
        if ( args != null && args.Activation)
        {
            inputClient.KeyDown(SoldatInputClient.Key.D);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Right On");
            #endif
        }
        else
        {
            inputClient.KeyUp(SoldatInputClient.Key.D);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Right Off");
            #endif
        }
    }
    void Fire(object sender, InputActionEventArgs args)
    {
        if ( args != null && args.Activation)
        {
            inputClient.MouseDownA(SoldatInputClient.MouseButton.Left);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Fire On");
            #endif
        }
        else
        {
            inputClient.MouseUp(SoldatInputClient.MouseButton.Left);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Fire Off");
            #endif
        }
    }
    void ChangeWeapon(object sender, InputActionEventArgs args)
    {
        if ( args != null && args.Activation)
        {
            inputClient.MouseDownA(SoldatInputClient.MouseButton.Middle);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("ChangeWeapon On");
            #endif
        }
        else
        {
            inputClient.MouseUp(SoldatInputClient.MouseButton.Middle);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("ChangeWeapon Off");
            #endif
        }
    }
    void Hook(object sender, InputActionEventArgs args)
    {
        if ( args != null && args.Activation)
        {
            inputClient.MouseDownA(SoldatInputClient.MouseButton.Right);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Hook On");
            #endif
        }
        else
        {
            inputClient.MouseUp(SoldatInputClient.MouseButton.Right);
            #if UNITY_EDITOR
            if(debugLogWhenDataSent)Debug.Log("Hook Off");
            #endif
        }
    }

}
