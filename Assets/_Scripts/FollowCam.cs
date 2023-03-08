using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    static private FollowCam S;

    static public GameObject POI;
    // static Point of Interest

    public enum eView { none, slingshot, castle, both };

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;    // [0,0]
    public GameObject viewBothGO;

    [Header("Dynamic")]
    public float camZ;
    public eView nextView = eView.slingshot;

    void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }

    //if (POI == null) return;    // Should there be no POI, return
    //Vector3 destination = POI.transform.position;   // Get the position of the POI

    void FixedUpdate()
    {
        Vector3 destination = Vector3.zero;
        if (POI != null)
        {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if ((poiRigid != null) && poiRigid.IsSleeping())
            {
                POI = null;
            }
        }
        if (POI != null)
        {
            destination = POI.transform.position;
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);    // Interpolate from the current camPosition towards dest
        destination.z = camZ;   // Force destionation.z to be camZ to keep the camera far away
        transform.position = destination;   // Set camera to the destination
        Camera.main.orthographicSize = destination.y + 10;  // Set the orthographicSize of the cam to keep ground in view
    }
    public void SwitchView(eView newView)
    {
        if (newView == eView.none)
        {
            newView = nextView;
        }
        switch (newView)
        {
            case eView.slingshot:
                POI = null;
                nextView = eView.castle;
                break;
            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();
                nextView = eView.both;
                break;
            case eView.both:
                POI = viewBothGO;
                nextView = eView.slingshot;
                break;
        }
    }
    public void SwitchView()
    {
        SwitchView(eView.none);
    }
    static public void SWITCH_VIEW(eView newView)
    {
        S.SwitchView(newView);
    }
}
