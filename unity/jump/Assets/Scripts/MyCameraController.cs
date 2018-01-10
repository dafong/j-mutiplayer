//using System;
//using DG.Tweening;
//using UnityEngine;
//
//// Token: 0x0200000F RID: 15
//public class MyCameraController : MonoBehaviour
//{
//	// Token: 0x060000EA RID: 234 RVA: 0x00007D90 File Offset: 0x00005F90
//	public void MoveCamera()
//	{
//		Vector3 a = this.gameC.target.transform.position - this.gameC.activeFloor.transform.position;
//		Vector3 vector = this.gameC.activeFloor.transform.position + a * 0.5f;
//		base.transform.DOMove(new Vector3(vector.x - 17f, 17f, vector.z - 17f), 1f, false);
//	}
//
//	// Token: 0x060000EB RID: 235 RVA: 0x00007E2C File Offset: 0x0000602C
//	public void MoveCamera(float movementTime)
//	{
//		Vector3 a = this.gameC.target.transform.position - this.gameC.activeFloor.transform.position;
//		Vector3 vector = this.gameC.activeFloor.transform.position + a * 0.5f;
//		base.transform.DOMove(new Vector3(vector.x - 17f, 17f, vector.z - 17f), movementTime, false);
//	}
//
//	// Token: 0x060000EC RID: 236 RVA: 0x00007EC4 File Offset: 0x000060C4
//	public void MoveCameraToCenter(Transform center)
//	{
//		base.transform.DOMove(new Vector3(center.position.x - 17f, base.transform.position.y, center.position.z - 17f), 1f, false).OnComplete(new TweenCallback(this.UnparentFloor));
//	}
//
//	// Token: 0x060000ED RID: 237 RVA: 0x00007F34 File Offset: 0x00006134
//	public void MoveCameraUp()
//	{
//		base.transform.DOMoveY(base.transform.position.y + 2.66f, 1f, false);
//	}
//
//	// Token: 0x060000EE RID: 238 RVA: 0x00007F6C File Offset: 0x0000616C
//	private void UnparentFloor()
//	{
//		this.gameC.UnparentFloor();
//	}
//
//	// Token: 0x040000BC RID: 188
//	public GameObject player;
//
//	// Token: 0x040000BD RID: 189
//	public GameController gameC;
//}
