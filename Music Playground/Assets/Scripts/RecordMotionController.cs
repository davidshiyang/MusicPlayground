using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecordMotionController : MonoBehaviour {

    bool isRec = false;
    bool doPlay = false;
    List<float> nums = new List<float>();
    float tempX;
    float tempY;
    float tempZ;
    float tempRotW;
    float tempRotX;
    float tempRotY;
    float tempRotZ;
    bool playedNoRep = false;
    public int leftSteamControllerIndex;
    public int rightSteamControllerIndex;
    public int thisSteamControllerIndex;
    public Vector3 startPosi;
    public Quaternion startRot;
    public GameObject parentHand;
    public bool loop = true;

    // Use this for initialization
    void Start()
    {
        leftSteamControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        rightSteamControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        parentHand = transform.parent.gameObject;
    }

    public void Record()
    {
        if (isRec == true)
        {
            isRec = false;
            transform.position = startPosi;
            print(startPosi);
        }
        else if (isRec == false)
        {
            startPosi = transform.position;
            isRec = true;
            doPlay = false;
        }
    }


    // Update is called once per frame
    public void Update()
    {

        if (SteamVR_Controller.Input(thisSteamControllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            transform.parent = parentHand.transform;
            transform.localPosition = new Vector3(0, 0, 0.1f);
            transform.localRotation = Quaternion.identity;
            Reset();
            Record();
        }

        if (SteamVR_Controller.Input(thisSteamControllerIndex).GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            transform.parent = null;
            RunIt();
        }


        if (playedNoRep == true)
        {
            doPlay = false;
        }



        if (isRec == true)
        {
            tempX = transform.position.x;
            tempY = transform.position.y;
            tempZ = transform.position.z;
            tempRotX = transform.rotation.x;
            tempRotY = transform.rotation.y;
            tempRotZ = transform.rotation.z;
            tempRotW = transform.rotation.w;

            nums.Add(tempX);
            nums.Add(tempY);
            nums.Add(tempZ);
            nums.Add(tempRotX);
            nums.Add(tempRotY);
            nums.Add(tempRotZ);
            nums.Add(tempRotW);
        }


        if (doPlay == true)
        {
            doPlay = false;
            StartCoroutine("Playback");
            Debug.Log(doPlay);
        }


    }


    public IEnumerator Playback()
    {

        playedNoRep = true;
        for (int i = 0; i < nums.Count; i += 7)
        {
            transform.position = new Vector3(nums[i], nums[i + 1], nums[i + 2]);
            transform.rotation = new Quaternion(nums[i + 3], nums[i + 4], nums[i + 5], nums[i + 6]);
            if (loop)
            {
                if (i == (nums.Count - 7))
                {
                    i = 0;
                }
            }

            yield return null;

        }

    }

    public void Reset()
    {
        nums.Clear();
    }

    public void RunIt()
    {
        isRec = false;
        doPlay = true;
        StartCoroutine("Playback");
    }

}
