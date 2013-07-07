using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LevelBuilder : MonoBehaviour {
	float r = .4f;
	float g = .4f;
	float b = .4f;
	
	public PlatformGenerator platformGeneratorPrefab;
	
	PlatformGenerator recentPlatformGenerator;
	int platformNumber = 0;
	float lastX = -10;
	float lastY = 0;
	void Awake() {
		Messenger.AddListener(typeof(PickupCollectedMessage),HandlePickupCollectedMessage);
		Messenger.AddListener(typeof(NextPlatformReachedMessage),HandleNextPlatformReachedMessage);
		Messenger.AddListener(typeof(PlayerDistanceMessage),HandlePlayerDistanceMessage);
	}
	void OnDestroy() {
		Messenger.RemoveListener(typeof(PickupCollectedMessage),HandlePickupCollectedMessage);
		Messenger.RemoveListener(typeof(NextPlatformReachedMessage),HandleNextPlatformReachedMessage);
		Messenger.RemoveListener(typeof(PlayerDistanceMessage),HandlePlayerDistanceMessage);
	}
	// Use this for initialization
	void Start () {
		CreatePlatform();
		CreatePlatform();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)) {
			CreatePlatform();
		}
		r = Mathf.Max(0,r-0.1f*Time.deltaTime);
		g = Mathf.Max(0,g-0.1f*Time.deltaTime);
		b = Mathf.Max(0,b-0.1f*Time.deltaTime);
		Messenger.Invoke(typeof(ColorMessage),new ColorMessage(r,g,b));
	}
	
	void CreatePlatform() {
		PlatformGenerator pg = (PlatformGenerator)Instantiate(platformGeneratorPrefab);
		pg.platformNumber = platformNumber++;
		pg.transform.parent = transform;
		pg.transform.position = new Vector3(lastX,0,0);
		pg.mLength = Random.Range(20,50);
		pg.GeneratePlatform();
		
		
		pg.transform.position = new Vector3(lastX,lastY - pg.StartY(),0);
		pg.SetColor(r,g,b);
		lastX = pg.EndX();
		lastY = pg.EndY();
		
		recentPlatformGenerator = pg;
		Messenger.Invoke(typeof(StageCreatedMessage),new StageCreatedMessage(pg.transform.position.x,pg.EndX()));
	}
	void HandlePickupCollectedMessage(Message msg) {
		PickupCollectedMessage message = msg as PickupCollectedMessage;
		if(message != null) {
			float plus = 0.75f;
			if(message.Pickup.pickupType == PickupType.R) {
				r = Mathf.Min(1,r+plus);
			}
			else if(message.Pickup.pickupType == PickupType.G) {
				g = Mathf.Min(1,g+plus);
			}
			else if(message.Pickup.pickupType == PickupType.B) {
				b = Mathf.Min(1,b+plus);
			}
			Messenger.Invoke(typeof(ColorMessage),new ColorMessage(r,g,b));
		}
	}
	void HandlePlayerDistanceMessage(Message msg) {
		PlayerDistanceMessage message = msg as PlayerDistanceMessage;
		if(message != null) {
			if(recentPlatformGenerator.EndX()-message.Distance < 15) {
				if(r > 0.1 || g > 0.1 || b > 0.1) {
					CreatePlatform();
				}
			}
		}
	}
	void HandleNextPlatformReachedMessage(Message msg) {
		NextPlatformReachedMessage message = msg as NextPlatformReachedMessage;
		if(message != null) {
			//if(r > 0.1 || g > 0.1 || b > 0.1) {
			//	CreatePlatform();
			//}
		}
	}
}
