using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public static class ControllerVelocity
{
	private static InputDevice _leftControllerDevice;
	private static InputDevice _rightControllerDevice;


	public static Vector3 GetControllerVelocity(bool isRight)
	{
		Vector3 _velocity;
		if (isRight)
		{ 
		if(_rightControllerDevice == null)
        {
			_rightControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		}
		Debug.Log(_rightControllerDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out _velocity));
		}
        else
        {
		if (_leftControllerDevice == null)
        {
			_leftControllerDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		}
            Debug.Log(_leftControllerDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out _velocity));
			
        }
			return _velocity;
	}
}