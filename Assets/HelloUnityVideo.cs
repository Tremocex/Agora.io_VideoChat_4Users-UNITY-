using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;

// this is an example of using Agora Unity SDK
// It demonstrates:
// How to enable video
// How to join/leave channel
// 
public class HelloUnityVideo : MonoBehaviour {

	// PLEASE KEEP THIS App ID IN SAFE PLACE
	// Get your own App ID at https://dashboard.agora.io/
	// After you entered the App ID, remove ## outside of Your App ID
	private static string appId = "ead1eb3456cc4e86863f80d3312dd528";

	// load agora engine
	public void loadEngine()
	{
		// start sdk
		Debug.Log ("initializeEngine");

		if (mRtcEngine != null) {
			Debug.Log ("Engine exists. Please unload it first!");
			return;
		}

		// init engine
		mRtcEngine = IRtcEngine.getEngine (appId);

		// enable log
		mRtcEngine.SetLogFilter (LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
	}

	public void join(string channel)
	{
		Debug.Log ("calling join (channel = " + channel + ")");

		if (mRtcEngine == null)
			return;

		// set callbacks (optional)
		mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
		mRtcEngine.OnUserJoined = onUserJoined;
		mRtcEngine.OnUserOffline = onUserOffline;

		// enable video
		mRtcEngine.EnableVideo();

		// allow camera output callback
		mRtcEngine.EnableVideoObserver();

		// join channel
		mRtcEngine.JoinChannel(channel, null, 0);

		Debug.Log ("initializeEngine done");
	}

	public string getSdkVersion () {
		return IRtcEngine.GetSdkVersion ();
	}

	public void leave()
	{
		Debug.Log ("calling leave");

		if (mRtcEngine == null)
			return;

		// leave channel
		mRtcEngine.LeaveChannel();
		// deregister video frame observers in native-c code
		mRtcEngine.DisableVideoObserver();
	}

	// unload agora engine
	public void unloadEngine()
	{
		Debug.Log ("calling unloadEngine");

		// delete
		if (mRtcEngine != null) {
			IRtcEngine.Destroy ();
			mRtcEngine = null;
		}
	}

	// accessing GameObject in Scnene1
	// set video transform delegate for statically created GameObject
	public void onSceneHelloVideoLoaded()
	{
		mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
		
	}

	// instance of agora engine
	public IRtcEngine mRtcEngine;

	// implement engine callbacks

	public uint mRemotePeer = 0; // insignificant. only record one peer
	public uint anf = 0;

	private void onJoinChannelSuccess (string channelName, uint uid, int elapsed)
	{
		Debug.Log ("JoinChannelSuccessHandler: uid = " + uid);
		GameObject textVersionGameObject = GameObject.Find ("VersionText");
		textVersionGameObject.GetComponent<Text> ().text = "Version : " + getSdkVersion ();




		GameObject go1,go2,go3,go4;

		GameObject go = GameObject.Find (uid.ToString ());
		if (!ReferenceEquals (go, null)) {
			return; // reuse
		}


		go1 = GameObject.Find("Cylinder");
		go2 = GameObject.Find("Cylinder2");
		go3 = GameObject.Find("Cylinder3");
		go4 = GameObject.Find("Cylinder4");
		
		go2.GetComponent<Renderer>().enabled = false;
		go3.GetComponent<Renderer>().enabled = false;
		go4.GetComponent<Renderer>().enabled = false;


		GameObject go1_cp= new GameObject();
		go1_cp = Instantiate(go1);

		go1_cp.GetComponent<Renderer>().enabled = true;

		go1.name = "C_"+uid.ToString();

		go1_cp.name  = uid.ToString();
		anf = uid;
		go = go1_cp;

		if (!ReferenceEquals (go, null)) {
			go.name = uid.ToString ();

			// configure videoSurface
			VideoSurface o = go.GetComponent<VideoSurface> ();
			o.SetForUser (uid);
			o.SetEnable (true);
			
		}
	}

	// When a remote user joined, this delegate will be called. Typically
	// create a GameObject to render video on it
	private void onUserJoined(uint uid, int elapsed)
	{
		Debug.Log ("onUserJoined: uid = " + uid);
		// this is called in main thread

		// find a game object to render video stream from 'uid'
		GameObject go = GameObject.Find (uid.ToString ());
		if (!ReferenceEquals (go, null)) {
			return; // reuse
		}

		// create a GameObject and assigne to this new user
		GameObject go1 = GameObject.Find("Cylinder");

		GameObject go2 = GameObject.Find("Cylinder2");
	

		GameObject go3 = GameObject.Find("Cylinder3");
		

		GameObject go4 = GameObject.Find("Cylinder4");
		

		if(!ReferenceEquals(go2,null)){
			Debug.Log("Entra no 2");
			GameObject go2_cp = new GameObject();
			go2_cp = Instantiate(go2);

			go2_cp.GetComponent<Renderer>().enabled = true;

			go2.name = "C2_"+uid.ToString();
			Debug.Log(uid.ToString());
			go2_cp.name  = uid.ToString();

			go = go2_cp;
			if (!ReferenceEquals (go, null)) {
				go.name = uid.ToString ();

				// configure videoSurface
				VideoSurface o = go.GetComponent<VideoSurface> ();
				o.SetForUser (uid);
				o.SetEnable (true);
				
			}
		}
			
		if(!ReferenceEquals(go3,null) && ReferenceEquals(go2,null)){
				GameObject go3_cp= new GameObject();
				go3_cp = Instantiate(go3);

				go3_cp.GetComponent<Renderer>().enabled = true;

				go3.name = "C3_"+uid.ToString();

				go3_cp.name  = uid.ToString();
				go = go3_cp;

				if (!ReferenceEquals (go, null)) {
					go.name = uid.ToString ();

					// configure videoSurface
					VideoSurface o = go.GetComponent<VideoSurface> ();
					o.SetForUser (uid);
					o.SetEnable (true);
					
				}
		}
		if(!ReferenceEquals(go4,null) && ReferenceEquals(go2,null) && ReferenceEquals(go3,null) && anf != uid){
				GameObject go4_cp= new GameObject();
				go4_cp = Instantiate(go4);

				go4_cp.GetComponent<Renderer>().enabled = true;
				go4.name = "C4_"+uid.ToString();


				go4_cp.name  = uid.ToString();
				go = go4_cp;

				if (!ReferenceEquals (go, null)) {
					go.name = uid.ToString ();

					// configure videoSurface
					VideoSurface o = go.GetComponent<VideoSurface> ();
					o.SetForUser (uid);
					o.SetEnable (true);
					
				}
		}

		mRemotePeer = uid;
	}

	// When remote user is offline, this delegate will be called. Typically
	// delete the GameObject for this user
	private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
	{
		// remove video stream
		Debug.Log ("onUserOffline: uid = " + uid);
		// this is called in main thread
		GameObject go = GameObject.Find (uid.ToString());
				

		GameObject go1 = GameObject.Find("C_"+uid.ToString());
	

		GameObject go2 = GameObject.Find("C2_"+uid.ToString());
		
		GameObject go3 = GameObject.Find("C3_"+uid.ToString());
		

		GameObject go4 = GameObject.Find("C4_"+uid.ToString());
		

		if (!ReferenceEquals(go1,null)) {
			Destroy (go);
			
			go1.GetComponent<Renderer>().enabled = false;
			go1.name = "Cylinder";
		}

		if (!ReferenceEquals(go2,null)) {
			Debug.Log("Entra");
			Destroy (go);

			go2.name = "Cylinder2";
			go2.GetComponent<Renderer>().enabled = false;

		}
		if (!ReferenceEquals(go3,null)) {
			Destroy (go);
			go3.name = "Cylinder3";
			go3.GetComponent<Renderer>().enabled = false;

		}
		if (!ReferenceEquals(go4,null)) {
			Destroy (go);

			go4.name = "Cylinder4";
			go4.GetComponent<Renderer>().enabled = false;

		}

		
	}

	
}
