using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {


	public Transform target;
	public float speed = 0.5f;
	Vector3[] path;
	public bool seekTargets = false, targetDetected = false;
	int targetIndex;


    void Update()
    {
		
		if(targetDetected) seekTargets = true;
		if (seekTargets) {
            seekTargets = false;
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound); 
		}


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collision");
            seekTargets = false;
            targetDetected = false;
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
           
        }
	}

	IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];
		while (targetIndex < path.Length) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					
					yield break;
				}
				currentWaypoint = path[targetIndex];
				
			}

            Debug.Log(currentWaypoint.x);
            Debug.Log(currentWaypoint.y);
            Debug.Log(currentWaypoint.z);
            transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
