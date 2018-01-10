//using System;
//using System.Collections;
//using DG.Tweening;
//using PathologicalGames;
//using UnityEngine;
//
//// Token: 0x02000011 RID: 17
//public class PlayerController : MonoBehaviour
//{
//	// Token: 0x060000F4 RID: 244 RVA: 0x00008074 File Offset: 0x00006274
//	private void Update()
//	{
//		if (!this.jumpings && (base.transform.eulerAngles.x != 0f || base.transform.eulerAngles.z != 0f))
//		{
//			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
//		}
//	}
//
//	// Token: 0x060000F5 RID: 245 RVA: 0x000080F4 File Offset: 0x000062F4
//	public void SetActiveFloor(GameObject newActiveFloor)
//	{
//		this.activeFloor = newActiveFloor;
//	}
//
//	// Token: 0x060000F6 RID: 246 RVA: 0x00008100 File Offset: 0x00006300
//	public void MovePlayer()
//	{
//		this.jumpings = true;
//		this.gameC.isJumping = true;
//		base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y + UnityEngine.Random.Range(-10f, 30f), 0f);
//		if (Mathf.Abs(this.gameC.movementDirection.x) < Mathf.Abs(this.gameC.movementDirection.z))
//		{
//			if (this.gameC.movementDirection.z > 0f)
//			{
//				base.transform.DORotate(new Vector3(-180f, 0f, 0f), 0.1f, RotateMode.WorldAxisAdd).SetEase(Ease.InCubic).OnComplete(new TweenCallback(this.RotateMore1));
//			}
//			else
//			{
//				base.transform.DORotate(new Vector3(180f, 0f, 0f), 0.1f, RotateMode.WorldAxisAdd).SetEase(Ease.InCubic).OnComplete(new TweenCallback(this.RotateMore3));
//			}
//		}
//		else if (this.gameC.movementDirection.x > 0f)
//		{
//			base.transform.DORotate(new Vector3(0f, 0f, 180f), 0.1f, RotateMode.WorldAxisAdd).SetEase(Ease.InCubic).OnComplete(new TweenCallback(this.RotateMore2));
//		}
//		else
//		{
//			base.transform.DORotate(new Vector3(0f, 0f, -180f), 0.1f, RotateMode.WorldAxisAdd).SetEase(Ease.InCubic).OnComplete(new TweenCallback(this.RotateMore4));
//		}
//		if (GameController.gameMode == 2)
//		{
//			base.transform.DOJump(new Vector3(base.transform.position.x + this.gameC.movementDirection.x * this.gameC.jumpPower, base.transform.position.y, base.transform.position.z + this.gameC.movementDirection.z * this.gameC.jumpPower), 7f + 2.66f * (float)this.gameC.score, 1, 0.4f, false).OnComplete(new TweenCallback(this.CheckFloor)).SetEase(Ease.Linear);
//		}
//		else
//		{
//			base.transform.DOJump(new Vector3(base.transform.position.x + this.gameC.movementDirection.x * this.gameC.jumpPower, 0.5f, base.transform.position.z + this.gameC.movementDirection.z * this.gameC.jumpPower), 7f, 1, 0.4f, false).OnComplete(new TweenCallback(this.CheckFloor)).SetEase(Ease.Linear);
//		}
//	}
//
//	// Token: 0x060000F7 RID: 247 RVA: 0x00008430 File Offset: 0x00006630
//	private void RotateMore1()
//	{
//		base.transform.DORotate(new Vector3(-180f, 0f, 0f), 0.3f, RotateMode.WorldAxisAdd).SetEase(Ease.InOutCubic).OnComplete(new TweenCallback(this.ResetRotation));
//	}
//
//	// Token: 0x060000F8 RID: 248 RVA: 0x0000847C File Offset: 0x0000667C
//	private void RotateMore2()
//	{
//		base.transform.DORotate(new Vector3(0f, 0f, 180f), 0.3f, RotateMode.WorldAxisAdd).SetEase(Ease.InOutCubic).OnComplete(new TweenCallback(this.ResetRotation));
//	}
//
//	// Token: 0x060000F9 RID: 249 RVA: 0x000084C8 File Offset: 0x000066C8
//	private void RotateMore3()
//	{
//		base.transform.DORotate(new Vector3(180f, 0f, 0f), 0.3f, RotateMode.WorldAxisAdd).SetEase(Ease.InOutCubic).OnComplete(new TweenCallback(this.ResetRotation));
//	}
//
//	// Token: 0x060000FA RID: 250 RVA: 0x00008514 File Offset: 0x00006714
//	private void RotateMore4()
//	{
//		base.transform.DORotate(new Vector3(0f, 0f, -180f), 0.3f, RotateMode.WorldAxisAdd).SetEase(Ease.InOutCubic).OnComplete(new TweenCallback(this.ResetRotation));
//	}
//
//	// Token: 0x060000FB RID: 251 RVA: 0x00008560 File Offset: 0x00006760
//	private void ResetRotation()
//	{
//		base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
//	}
//
//	// Token: 0x060000FC RID: 252 RVA: 0x0000859C File Offset: 0x0000679C
//	private void RandomizeRotation()
//	{
//		if (UnityEngine.Random.Range(0, 2) == 0)
//		{
//			this.toRotate = 90f;
//		}
//		else
//		{
//			this.toRotate = 180f;
//		}
//	}
//
//	// Token: 0x060000FD RID: 253 RVA: 0x000085C8 File Offset: 0x000067C8
//	public void SpawnParticles()
//	{
//		this.soundC.PlaySound("land");
//		PoolManager.Pools["MainPool"].Spawn(this.particles.transform, base.transform.position - new Vector3(0f, 0.75f, 0f), Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
//	}
//
//	// Token: 0x060000FE RID: 254 RVA: 0x00008644 File Offset: 0x00006844
//	private void OnTriggerEnter(Collider col)
//	{
//		if (col.transform.tag == "Gem")
//		{
//			if (!this.bonusLevelActive)
//			{
//				this.gameC.GemCollected(1);
//				PoolManager.Pools["MainPool"].Spawn(this.gemSprite.transform, col.transform.position, Quaternion.Euler(new Vector3(35f, 45f, 0f)));
//				PoolManager.Pools["MainPool"].Spawn(this.gemParticles.transform, col.transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
//			}
//			else
//			{
//				this.gameC.IncreaseBonusGemCounter();
//				this.gameC.SpawnGemPlusXSprite(col.transform.position);
//				this.gameC.SpawnNewBonusLevelGem();
//			}
//			this.soundC.PlaySound("gemCollect");
//			col.transform.parent = null;
//			col.transform.position = new Vector3(100f, 0f, 0f);
//			PoolManager.Pools["MainPool"].Despawn(col.transform);
//		}
//	}
//
//	// Token: 0x060000FF RID: 255 RVA: 0x00008790 File Offset: 0x00006990
//	private void BowlingGem()
//	{
//		this.gameC.GemCollected(1);
//		PoolManager.Pools["MainPool"].Spawn(this.gemSprite.transform, base.transform.position, Quaternion.Euler(new Vector3(35f, 45f, 0f)));
//		PoolManager.Pools["MainPool"].Spawn(this.gemParticles.transform, base.transform.position, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
//	}
//
//	// Token: 0x06000100 RID: 256 RVA: 0x00008834 File Offset: 0x00006A34
//	private void CheckFloor()
//	{
//		if (GameController.gameMode == 3)
//		{
//			this.jumpings = true;
//			base.StartCoroutine("WaitAndReset");
//		}
//		else
//		{
//			this.jumpings = false;
//		}
//		if (GameController.gameMode == 2)
//		{
//			base.transform.position = new Vector3(base.transform.position.x, 0.5f + (float)this.gameC.score * 2.66f, base.transform.position.z);
//		}
//		else
//		{
//			base.transform.position = new Vector3(base.transform.position.x, 0.5f, base.transform.position.z);
//		}
//		if (Physics.Raycast(base.transform.position, Vector3.down, out this.hit, 2f))
//		{
//			this.SpawnParticles();
//			if (GameController.gameMode == 3)
//			{
//				this.gameC.isJumping = true;
//			}
//			else
//			{
//				this.gameC.isJumping = false;
//			}
//			if (this.hit.transform.gameObject != this.activeFloor && this.hit.transform.tag == "Floor")
//			{
//				if ((GameController.gameMode == 1 || GameController.gameMode == 4) && !this.bonusLevelActive)
//				{
//					if (this.gameC.score == 0 && GameController.gameMode == 4)
//					{
//						this.gameC.activeFloor.GetComponent<SpeedFloor>().StartMoveDownCoroutine();
//					}
//					this.gameC.UpdateScore(1, true);
//					this.gameC.RandomizeNextFloorPosition();
//					this.gameC.SpawnFloor(true);
//					this.gameC.PerfectHit();
//					this.gameC.ResetFastJumpCounter();
//				}
//				if (GameController.gameMode == 4)
//				{
//					this.hit.transform.GetComponent<SpeedFloor>().StartBlinking();
//				}
//				if (GameController.gameMode == 2)
//				{
//					if (this.gameC.score != 0)
//					{
//						this.gameC.GameOver();
//					}
//					else
//					{
//						this.gameC.SwapStackingBottles();
//						this.gameC.UpdateScore(1, true);
//					}
//				}
//				if (GameController.gameMode != 2)
//				{
//					this.activeFloor = this.hit.transform.gameObject;
//					this.gameC.ImpactBounce();
//				}
//			}
//			if (GameController.gameMode == 2 && this.hit.transform.gameObject != this.activeFloor && this.hit.transform.tag == "StackElement")
//			{
//				this.gameC.SwapStackingBottles();
//				this.gameC.UpdateScore(1, true);
//			}
//		}
//		else
//		{
//			this.jumpings = true;
//			this.SetGravityActive();
//			if (!this.bonusLevelActive)
//			{
//				this.gameC.GameOver();
//				if (GameController.gameMode == 3)
//				{
//					base.StopCoroutine("WaitAndReset");
//				}
//			}
//			else
//			{
//				this.gameC.BonusGameOver();
//				this.soundC.PlaySound("gameOver");
//			}
//		}
//	}
//
//	// Token: 0x06000101 RID: 257 RVA: 0x00008B80 File Offset: 0x00006D80
//	private IEnumerator WaitAndReset()
//	{
//		yield return new WaitForSeconds(2f);
//		GameObject[] tempPins = GameObject.FindGameObjectsWithTag("Pin");
//		int tempScore = 0;
//		for (int i = 0; i < tempPins.Length; i++)
//		{
//			if ((tempPins[i].transform.localEulerAngles.x > 10f && tempPins[i].transform.localEulerAngles.x < 350f) || (tempPins[i].transform.localEulerAngles.z > 10f && tempPins[i].transform.localEulerAngles.z < 350f) || tempPins[i].transform.position.y < -2f)
//			{
//				tempScore++;
//			}
//		}
//		this.gameC.UpdateScore(tempScore, false);
//		if (tempScore == 10)
//		{
//			this.BowlingGem();
//		}
//		yield return new WaitForSeconds(0.5f);
//		if (tempScore == 10)
//		{
//			this.gameC.ResetAfterStrike();
//		}
//		else if (this.gameC.bowlingCounter == 1)
//		{
//			this.gameC.bowlingCounter++;
//			this.ResetPlayer();
//			this.gameC.bowlingRoundCounterText.text = "2/2";
//		}
//		else
//		{
//			this.gameC.GameOver();
//		}
//		yield break;
//	}
//
//	// Token: 0x06000102 RID: 258 RVA: 0x00008B9C File Offset: 0x00006D9C
//	public void ResetPlayer()
//	{
//		base.transform.position = new Vector3(0f, 0.5f, 0f);
//		this.richie.isKinematic = true;
//		this.richie.useGravity = false;
//		base.transform.localEulerAngles = new Vector3(0f, (float)UnityEngine.Random.Range(0, 360), 0f);
//		base.transform.localScale = Vector3.zero;
//		if (GameController.gameMode == 3)
//		{
//			base.transform.localScale = Vector3.one;
//			this.gameC.isJumping = false;
//			this.bowlingCollider.enabled = true;
//		}
//		this.jumpings = false;
//	}
//
//	// Token: 0x06000103 RID: 259 RVA: 0x00008C50 File Offset: 0x00006E50
//	public void EquipCharacter(GameObject newChar)
//	{
//		newChar.transform.parent = base.transform;
//		newChar.transform.localPosition = Vector3.zero - new Vector3(0f, 0.75f, 0f);
//		newChar.transform.localEulerAngles += new Vector3(0f, 90f, 0f);
//		newChar.transform.localScale = Vector3.one;
//		if (GameController.gameMode == 2)
//		{
//			base.transform.localEulerAngles = Vector3.zero;
//			newChar.transform.localEulerAngles = Vector3.zero;
//		}
//		if (this.characterMesh != null)
//		{
//			UnityEngine.Object.Destroy(this.characterMesh);
//			this.characterMesh = null;
//		}
//		this.characterMesh = newChar;
//	}
//
//	// Token: 0x06000104 RID: 260 RVA: 0x00008D28 File Offset: 0x00006F28
//	public void SetBonusLevelActive(bool active)
//	{
//		this.bonusLevelActive = active;
//	}
//
//	// Token: 0x06000105 RID: 261 RVA: 0x00008D34 File Offset: 0x00006F34
//	public void SetGravityActive()
//	{
//		this.richie.isKinematic = false;
//		this.richie.useGravity = true;
//		base.GetComponent<BoxCollider>().isTrigger = false;
//		if (GameController.gameMode == 3)
//		{
//			this.bowlingCollider.enabled = false;
//		}
//	}
//
//	// Token: 0x06000106 RID: 262 RVA: 0x00008D7C File Offset: 0x00006F7C
//	public void RemoveStackingCharacter()
//	{
//		if (this.characterMesh != null)
//		{
//			this.characterMesh.transform.eulerAngles = Vector3.zero;
//			this.characterMesh.transform.parent = null;
//			this.gameC.StackingImpactBounce(this.characterMesh.gameObject);
//		}
//	}
//
//	// Token: 0x06000107 RID: 263 RVA: 0x00008DD8 File Offset: 0x00006FD8
//	public void EquipStackingCharacter(GameObject newChar)
//	{
//		base.transform.localEulerAngles = Vector3.zero;
//		newChar.transform.parent = base.transform;
//		newChar.transform.localPosition = Vector3.zero - new Vector3(0f, 0.75f, 0f);
//		newChar.transform.localEulerAngles = Vector3.zero;
//		newChar.transform.localScale = Vector3.zero;
//		newChar.transform.DOScale(1f, 0.2f);
//		this.characterMesh = newChar;
//	}
//
//	// Token: 0x040000C0 RID: 192
//	public GameController gameC;
//
//	// Token: 0x040000C1 RID: 193
//	public GameObject particles;
//
//	// Token: 0x040000C2 RID: 194
//	public Rigidbody richie;
//
//	// Token: 0x040000C3 RID: 195
//	private RaycastHit hit;
//
//	// Token: 0x040000C4 RID: 196
//	private GameObject activeFloor;
//
//	// Token: 0x040000C5 RID: 197
//	public SoundController soundC;
//
//	// Token: 0x040000C6 RID: 198
//	public GameObject gemParticles;
//
//	// Token: 0x040000C7 RID: 199
//	private GameObject characterMesh;
//
//	// Token: 0x040000C8 RID: 200
//	public GameObject gemSprite;
//
//	// Token: 0x040000C9 RID: 201
//	private bool bonusLevelActive;
//
//	// Token: 0x040000CA RID: 202
//	private float toRotate = 90f;
//
//	// Token: 0x040000CB RID: 203
//	private bool jumpings;
//
//	// Token: 0x040000CC RID: 204
//	public SphereCollider bowlingCollider;
//}
