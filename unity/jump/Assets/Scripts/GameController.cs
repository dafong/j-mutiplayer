//using System;
//using System.Collections;
//using System.Collections.Generic;
//using CodeStage.AntiCheat.ObscuredTypes;
//using DG.Tweening;
//using DG.Tweening.Core;
//using DG.Tweening.Plugins.Options;
//using Heyzap;
//using I2.Loc;
//using PathologicalGames;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Rendering;
//
//// Token: 0x0200000B RID: 11
//public class GameController : MonoBehaviour
//{
//	// Token: 0x06000038 RID: 56 RVA: 0x00002CC8 File Offset: 0x00000EC8
//	private void Awake()
//	{
//		this.adController = base.transform.GetComponent<AdScript>();
//		if (CryptoPlayerPrefs.GetInt("IsPro", 0) == 1)
//		{
//			this.isPro = true;
//		}
//	}
//
//	// Token: 0x06000039 RID: 57 RVA: 0x00002D04 File Offset: 0x00000F04
//	private void Start()
//	{
//		DOTween.defaultRecyclable = true;
//		DOTween.useSafeMode = false;
//		this.texBG = new Texture2D(1, 1, TextureFormat.RGB24, false);
//		Application.targetFrameRate = 60;
//		Time.timeScale = 1.05f;
//		base.StartCoroutine("CheckChinese");
//		base.StartCoroutine("DisableGOs");
//		this.LoadVariables();
//		this.floorList = new List<Transform>();
//		this.activeFloorMat = this.floorMats[0];
//		this.player.transform.localScale = Vector3.zero;
//		this.SpawnFirstFloor();
//		this.RandomizeNextFloorPosition();
//		this.SpawnFloor(true);
//		PoolManager.Pools["MainPool"].Spawn(this.perfectHitSquare.transform, new Vector3(100f, 0.2f, 0f), Quaternion.identity);
//		base.StartCoroutine("ShowMainMenu");
//		this.ShowMenuGemCount();
//		base.StartCoroutine("ScalePlayer");
//		if (CryptoPlayerPrefs.GetInt("Volume", 1) == 0)
//		{
//			this.SoundOnOff();
//		}
//		if (CryptoPlayerPrefs.GetInt("FacebookLike", 0) == 1)
//		{
//			UnityEngine.Object.Destroy(this.facebookBtn);
//		}
//		base.StartCoroutine("DelayedPlayServiceLogin");
//		SA_Singleton<UM_InAppPurchaseManager>.Instance.Init();
//		GoogleAnalytics.StartTracking();
//		if (!this.isPro)
//		{
//		}
//		this.floor2BGMat.color = this.floorColors2[UnityEngine.Random.Range(0, this.floorColors2.Length)];
//		base.StartCoroutine("ReadMenuPixelColor");
//	}
//
//	// Token: 0x0600003A RID: 58 RVA: 0x00002E88 File Offset: 0x00001088
//	private IEnumerator DelayedPlayServiceLogin()
//	{
//		yield return new WaitForSeconds(1f);
//		SA_Singleton<GooglePlayConnection>.instance.connect();
//		yield break;
//	}
//
//	// Token: 0x0600003B RID: 59 RVA: 0x00002E9C File Offset: 0x0000109C
//	private IEnumerator DisableGOs()
//	{
//		yield return 0;
//		yield return 0;
//		this.perfectLabel.gameObject.SetActive(false);
//		this.fastLabel.gameObject.SetActive(false);
//		yield break;
//	}
//
//	// Token: 0x0600003C RID: 60 RVA: 0x00002EB8 File Offset: 0x000010B8
//	public void EnableGameMode(int gameModeNumber)
//	{
//		GameController.gameMode = gameModeNumber;
//		this.playerC.bowlingCollider.enabled = false;
//		PoolManager.Pools["MainPool"].DespawnAll();
//		if (this.tempPins != null)
//		{
//			UnityEngine.Object.Destroy(this.tempPins);
//			this.tempPins = null;
//		}
//		if (this.stackBGWall.activeInHierarchy)
//		{
//			this.stackBGWall.SetActive(false);
//		}
//		if (GameController.gameMode == 1)
//		{
//			this.ResetGame();
//		}
//		else if (GameController.gameMode == 2)
//		{
//			this.ResetGame();
//			this.stackBGWall.SetActive(true);
//		}
//		else if (GameController.gameMode == 3)
//		{
//			this.bowlingCounter = 0;
//			this.playerC.bowlingCollider.enabled = true;
//			this.ResetGame();
//		}
//		else if (GameController.gameMode == 4)
//		{
//			this.ResetGame();
//		}
//		this.BackFromGameModeSelect();
//	}
//
//	// Token: 0x0600003D RID: 61 RVA: 0x00002FAC File Offset: 0x000011AC
//	private void Update()
//	{
//		if (Input.GetMouseButtonDown(0) && !this.gameOver && !this.isJumping && this.gameStarted && !this.tutorialActive)
//		{
//			if (!this.startButtonPressed)
//			{
//				this.startButtonPressed = true;
//			}
//			base.StartCoroutine("ChargeUp");
//			this.didCharge = true;
//		}
//		if (Input.GetMouseButtonUp(0) && !this.gameOver && !this.isJumping && this.didCharge && this.gameStarted && !this.tutorialActive)
//		{
//			this.StartJumpAction();
//		}
//		if (this.fastJumpCounter > 0f && this.fastJumpCounterActive)
//		{
//			this.fastJumpCounter -= Time.deltaTime;
//		}
//		else if (this.fastJumpCounterActive)
//		{
//			this.fastJumpCounterActive = false;
//		}
//		if (Input.GetKeyDown(KeyCode.Escape))
//		{
//			if (!this.pressedBack)
//			{
//				base.StartCoroutine("PressedBackBtn");
//				AndroidToast.ShowToastNotification("Press again to quit");
//			}
//			else
//			{
//				Application.Quit();
//			}
//		}
//	}
//
//	// Token: 0x0600003E RID: 62 RVA: 0x000030D8 File Offset: 0x000012D8
//	private IEnumerator PressedBackBtn()
//	{
//		this.pressedBack = true;
//		yield return new WaitForSeconds(2f);
//		this.pressedBack = false;
//		yield break;
//	}
//
//	// Token: 0x0600003F RID: 63 RVA: 0x000030F4 File Offset: 0x000012F4
//	private void StartJumpAction()
//	{
//		if (GameController.gameMode == 4)
//		{
//			this.activeFloor.GetComponent<SpeedFloor>().StopBlinking();
//		}
//		base.StopCoroutine("ChargeUp");
//		DOTween.Kill("targetpointer", false);
//		this.targetpointer.SetActive(false);
//		this.chargeParticles.SetActive(false);
//		DOTween.Kill("ScaleUp", false);
//		DOTween.Kill("MoveDown", false);
//		DOTween.Kill("MoveDownFloor", false);
//		DOTween.Kill("ScaleFloor", false);
//		this.player.transform.DOScale(1f, 0.5f);
//		if (!GameController.bonusLevelActive)
//		{
//			if (GameController.gameMode != 2)
//			{
//				this.activeFloor.transform.DOMoveY(-0.1f, 0.35f, false).SetEase(Ease.OutBounce);
//			}
//			else
//			{
//				this.activeFloor.transform.DOMoveY(-0.1f + 2.66f * (float)this.score, 0.35f, false).SetEase(Ease.OutBounce);
//			}
//			this.activeFloor.transform.DOScaleY(1f, 0.35f).SetEase(Ease.OutBounce);
//		}
//		else
//		{
//			this.activeFloor.transform.DOMoveY(-8f, 0.35f, false).SetEase(Ease.OutBounce);
//		}
//		Vector3 vector = new Vector3(this.target.transform.position.x - this.player.transform.position.x, 0f, this.target.transform.position.z - this.player.transform.position.z);
//		this.movementDirection = vector.normalized;
//		if (GameController.gameMode == 2)
//		{
//			this.player.transform.position = new Vector3(this.player.transform.position.x, 0.5f + (float)this.score * 2.66f, this.player.transform.position.z);
//		}
//		this.playerC.MovePlayer();
//		if (this.savedMaxJumpPower < this.jumpPower)
//		{
//			this.savedMaxJumpPower = this.jumpPower;
//		}
//		this.jumpPower = 0f;
//		this.soundC.StopCharging();
//		this.didCharge = false;
//	}
//
//	// Token: 0x06000040 RID: 64 RVA: 0x00003380 File Offset: 0x00001580
//	private void StartNewGame()
//	{
//		HeyzapAds.PauseExpensiveWork();
//		this.adController.gameCounter++;
//		this.adController.ShowBanner(false);
//		this.roundsPlayed = ++this.roundsPlayed;
//		this.soundC.PlaySound("startNewGameSound");
//		this.startGameCollider.SetActive(false);
//		this.gameStarted = true;
//		this.gameOver = false;
//		this.loopTapText = false;
//		base.StopCoroutine("FadeTapTextCoroutine");
//		this.HideMainMenu();
//		if (!this.tutorialActive || GameController.gameMode != 1)
//		{
//			base.StartCoroutine("ShowScoreText");
//		}
//		else
//		{
//			base.StartCoroutine("StartTutorial");
//		}
//		if (GameController.gameMode == 3)
//		{
//			this.ShowBowlingRoundCounter();
//		}
//		this.HideMenuGemCount();
//	}
//
//	// Token: 0x06000041 RID: 65 RVA: 0x00003450 File Offset: 0x00001650
//	private void LoadVariables()
//	{
//		this.gemCount = CryptoPlayerPrefs.GetInt("GemCount", 0);
//		this.activeCharacter = CryptoPlayerPrefs.GetInt("ActiveCharacter", 0);
//		if (GameController.gameMode == 1)
//		{
//			this.bestScore = CryptoPlayerPrefs.GetInt("BestScore", 0);
//			this.playerC.EquipCharacter(UnityEngine.Object.Instantiate<GameObject>(this.characters[this.activeCharacter]));
//			this.roundsPlayed = CryptoPlayerPrefs.GetInt("RoundsPlayed", 0);
//			this.bonusGamesPlayed = CryptoPlayerPrefs.GetInt("BonusGamesPlayed", 0);
//			this.SetBonusLevelGoal();
//		}
//		else if (GameController.gameMode == 2)
//		{
//			this.bestScore = CryptoPlayerPrefs.GetInt("BestScoreGameMode2", 0);
//			this.playerC.EquipCharacter(UnityEngine.Object.Instantiate<GameObject>(this.stackingBottle));
//		}
//		else if (GameController.gameMode == 3)
//		{
//			this.bestScore = CryptoPlayerPrefs.GetInt("BestScoreGameMode3", 0);
//			this.playerC.EquipCharacter(UnityEngine.Object.Instantiate<GameObject>(this.characters[this.activeCharacter]));
//		}
//		else if (GameController.gameMode == 4)
//		{
//			this.bestScore = CryptoPlayerPrefs.GetInt("BestScoreGameMode4", 0);
//			this.playerC.EquipCharacter(UnityEngine.Object.Instantiate<GameObject>(this.characters[this.activeCharacter]));
//		}
//		if (CryptoPlayerPrefs.GetInt("ShowTutorial", 1) == 1 && GameController.gameMode == 1)
//		{
//			this.tutorialActive = true;
//		}
//		else
//		{
//			this.tutorialActive = false;
//		}
//		this.bonusLevelCounter = CryptoPlayerPrefs.GetInt("BonusLevelCounter", 0);
//		this.cheapestPrice = CryptoPlayerPrefs.GetInt("CheapestPrice", 100);
//		this.unlockedCharacters = CryptoPlayerPrefs.GetInt("UnlockedCharacters", 0);
//	}
//
//	// Token: 0x06000042 RID: 66 RVA: 0x00003600 File Offset: 0x00001800
//	private void ShowBowlingRoundCounter()
//	{
//		this.bowlingRoundCounterText.transform.DOLocalMoveY(7.5f, 0.5f, false);
//		this.bowlingRoundCounterText.text = "1/2";
//	}
//
//	// Token: 0x06000043 RID: 67 RVA: 0x0000363C File Offset: 0x0000183C
//	private void HideBowlingRoundCounter()
//	{
//		this.bowlingRoundCounterText.transform.DOLocalMoveY(15f, 0.5f, false);
//	}
//
//	// Token: 0x06000044 RID: 68 RVA: 0x0000365C File Offset: 0x0000185C
//	private void SetBonusLevelGoal()
//	{
//		if (this.bonusGamesPlayed == 0)
//		{
//			this.bonusLevelCounterGoal = 50f;
//		}
//		else if (this.bonusGamesPlayed == 1)
//		{
//			this.bonusLevelCounterGoal = 100f;
//		}
//		else if (this.bonusGamesPlayed == 2)
//		{
//			this.bonusLevelCounterGoal = 200f;
//		}
//		else if (this.bonusGamesPlayed == 3)
//		{
//			this.bonusLevelCounterGoal = 325f;
//		}
//		else if (this.bonusGamesPlayed == 4)
//		{
//			this.bonusLevelCounterGoal = 450f;
//		}
//		else if (this.bonusGamesPlayed == 5)
//		{
//			this.bonusLevelCounterGoal = 600f;
//		}
//	}
//
//	// Token: 0x06000045 RID: 69 RVA: 0x0000370C File Offset: 0x0000190C
//	private IEnumerator SaveVariables()
//	{
//		CryptoPlayerPrefs.SetInt("GemCount", this.gemCount);
//		if (this.score > this.bestScore)
//		{
//			this.bestScore = this.score;
//			if (GameController.gameMode == 1)
//			{
//				CryptoPlayerPrefs.SetInt("BestScore", this.bestScore);
//			}
//			else if (GameController.gameMode == 2)
//			{
//				CryptoPlayerPrefs.SetInt("BestScoreGameMode2", this.bestScore);
//			}
//			else if (GameController.gameMode == 3)
//			{
//				CryptoPlayerPrefs.SetInt("BestScoreGameMode3", this.bestScore);
//			}
//			else if (GameController.gameMode == 4)
//			{
//				CryptoPlayerPrefs.SetInt("BestScoreGameMode4", this.bestScore);
//			}
//		}
//		if (GameController.gameMode == 1)
//		{
//			this.CheckAndSubmitAchievements();
//			this.bonusLevelCounter += this.score;
//			CryptoPlayerPrefs.SetInt("BonusLevelCounter", this.bonusLevelCounter);
//		}
//		else if (GameController.gameMode == 2)
//		{
//			this.CheckAndSubmitAchievementsStacking();
//		}
//		else if (GameController.gameMode == 3)
//		{
//			this.CheckAndSubmitAchievementsBowling();
//		}
//		else if (GameController.gameMode == 4)
//		{
//			this.CheckAndSubmitAchievementsSpeed();
//		}
//		CryptoPlayerPrefs.SetInt("RoundsPlayed", this.roundsPlayed);
//		this.CheckAndSubmitRoundsPlayed();
//		yield return new WaitForSeconds(0.2f);
//		if (GameController.gameMode == 1 && this.score >= 30 && CryptoPlayerPrefs.GetInt("ShowPopup", 1) == 1)
//		{
//			this.showingPopup = true;
//			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				MobileNativeRateUs ratePopUp = new MobileNativeRateUs(ScriptLocalization.Get("SHAREHEADLINE"), ScriptLocalization.Get("SHARETEXT"));
//				MobileNativeRateUs mobileNativeRateUs = ratePopUp;
//				mobileNativeRateUs.OnComplete = (Action<MNDialogResult>)Delegate.Combine(mobileNativeRateUs.OnComplete, new Action<MNDialogResult>(this.OnRatePopUpClose));
//				ratePopUp.SetAppleId("1178454068");
//				ratePopUp.SetAndroidAppUrl("https://play.google.com/store/apps/details?id=com.ketchapp.bottleflip");
//				ratePopUp.Start();
//			}
//		}
//		yield break;
//	}
//
//	// Token: 0x06000046 RID: 70 RVA: 0x00003728 File Offset: 0x00001928
//	private IEnumerator WobbleActiveFloor()
//	{
//		Transform tempFloor = this.activeFloor.transform;
//		yield return new WaitForSeconds(0.1f);
//		tempFloor.DOScaleY(tempFloor.localScale.y, 0.2f).SetEase(Ease.OutBounce);
//		yield break;
//	}
//
//	// Token: 0x06000047 RID: 71 RVA: 0x00003744 File Offset: 0x00001944
//	public void RandomizeNextFloorPosition()
//	{
//		if (GameController.gameMode == 1)
//		{
//			if (UnityEngine.Random.Range(1, 3) == 1)
//			{
//				this.nextFloorScale = 0.7f;
//			}
//			else
//			{
//				this.nextFloorScale = 0.900000036f;
//			}
//		}
//		else if (GameController.gameMode == 2)
//		{
//			this.nextFloorScale = 0.7f;
//		}
//		else if (GameController.gameMode == 3)
//		{
//			this.nextFloorScale = 1.5f;
//		}
//		else if (GameController.gameMode == 4)
//		{
//			if (UnityEngine.Random.Range(1, 3) == 1)
//			{
//				this.nextFloorScale = 0.6f;
//			}
//			else
//			{
//				this.nextFloorScale = 0.8f;
//			}
//		}
//		if (!this.tutorialActive)
//		{
//			this.floorsCounter++;
//			if ((float)(this.floorsCounter - 1) * 0.04f * 0.2f < 0.450000018f)
//			{
//				this.nextFloorScale -= (float)this.floorsCounter * 0.04f * 0.2f;
//			}
//			else
//			{
//				this.nextFloorScale -= 0.450000018f;
//			}
//		}
//		this.random = UnityEngine.Random.Range(1, 3);
//		if (GameController.gameMode == 1)
//		{
//			if (this.random == 1)
//			{
//				this.spawner.transform.position += new Vector3(this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 14f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f), 0f, 0f);
//			}
//			else
//			{
//				this.spawner.transform.position += new Vector3(0f, 0f, this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 14f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f));
//			}
//		}
//		else if (GameController.gameMode == 2)
//		{
//			if (this.random == 1)
//			{
//				this.spawner.transform.position += new Vector3(this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 7f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f), 0f, 0f);
//			}
//			else
//			{
//				this.spawner.transform.position += new Vector3(0f, 0f, this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 7f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f));
//			}
//		}
//		else if (GameController.gameMode == 3)
//		{
//			if (this.bowlingCounter == 0)
//			{
//				if (this.random == 1)
//				{
//					this.spawner.transform.position += new Vector3(this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 14f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f), 0f, 0f);
//				}
//				else
//				{
//					this.spawner.transform.position += new Vector3(0f, 0f, this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 14f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f));
//				}
//				this.bowlingSpawnerPos = this.spawner.transform.position;
//				this.bowlingSavedRandom = this.random;
//			}
//			else
//			{
//				this.spawner.transform.position = this.bowlingSpawnerPos;
//				this.random = this.bowlingSavedRandom;
//			}
//		}
//		else if (GameController.gameMode == 4)
//		{
//			if (this.random == 1)
//			{
//				this.spawner.transform.position += new Vector3(this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 11f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f), 0f, 0f);
//			}
//			else
//			{
//				this.spawner.transform.position += new Vector3(0f, 0f, this.lastFloorScale / 2f * 5f + this.nextFloorScale / 2f * 5f + UnityEngine.Random.Range(0f, 11f - this.nextFloorScale / 2f * 5f - this.lastFloorScale / 2f * 5f));
//			}
//		}
//		if (GameController.gameMode == 3)
//		{
//			base.StartCoroutine("SpawnPins", this.random);
//		}
//	}
//
//	// Token: 0x06000048 RID: 72 RVA: 0x00003D8C File Offset: 0x00001F8C
//	private IEnumerator SpawnPins(int randomi)
//	{
//		if (this.bowlingCounter == 0 && this.tempPins != null)
//		{
//			UnityEngine.Object.Destroy(this.tempPins);
//			this.tempPins = null;
//		}
//		yield return new WaitForSeconds(0.5f);
//		if (this.bowlingCounter == 0)
//		{
//			this.tempPins = (UnityEngine.Object.Instantiate(this.bowlingPins[UnityEngine.Random.Range(0, this.bowlingPins.Length)], this.spawner.transform.position - new Vector3(0f, 0.1f, 0f), Quaternion.identity) as GameObject);
//			this.bowlingCounter++;
//			this.soundC.PlaySound("buttonSound");
//		}
//		else
//		{
//			this.tempPins.SetActive(true);
//			this.bowlingCounter = 0;
//		}
//		if (randomi != 1)
//		{
//			this.tempPins.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
//		}
//		yield break;
//	}
//
//	// Token: 0x06000049 RID: 73 RVA: 0x00003DB8 File Offset: 0x00001FB8
//	public void SetTempPinsInactive()
//	{
//		this.tempPins.SetActive(false);
//	}
//
//	// Token: 0x0600004A RID: 74 RVA: 0x00003DC8 File Offset: 0x00001FC8
//	public void SpawnFloor(bool moveCam)
//	{
//		Vector3 vector = this.spawner.transform.position;
//		if (this.startButtonPressed)
//		{
//			this.activeFloor = this.target;
//		}
//		vector -= new Vector3(0f, 0.1f, 0f);
//		if (GameController.gameMode == 1)
//		{
//			int num = UnityEngine.Random.Range(0, this.floorArray.Length);
//			this.target = PoolManager.Pools["MainPool"].Spawn(this.floorArray[num], vector, Quaternion.identity).gameObject;
//			if (UnityEngine.Random.Range(0, 2) == 0)
//			{
//				this.target.transform.eulerAngles += new Vector3(0f, 90f, 0f);
//			}
//			if (this.targetPointerColorArray[num])
//			{
//				this.targetPointerSprite.color = new Color(1f, 1f, 1f, 0.5f);
//			}
//			else
//			{
//				this.targetPointerSprite.color = new Color(0f, 0f, 0f, 0.5f);
//			}
//		}
//		else if (GameController.gameMode == 2)
//		{
//			this.target = PoolManager.Pools["MainPool"].Spawn(this.stackTargetTable, vector, Quaternion.identity).gameObject;
//			this.camC.MoveCameraToCenter(this.target.transform);
//		}
//		else if (GameController.gameMode == 3)
//		{
//			this.target = PoolManager.Pools["MainPool"].Spawn(this.bowlingTable, vector, Quaternion.identity).gameObject;
//		}
//		else if (GameController.gameMode == 4)
//		{
//			this.target = PoolManager.Pools["MainPool"].Spawn(this.speedFloor, vector, Quaternion.identity).gameObject;
//		}
//		if (moveCam && GameController.gameMode != 2)
//		{
//			this.camC.MoveCamera();
//		}
//		else if (moveCam && GameController.gameMode == 2)
//		{
//			this.camC.MoveCameraUp();
//		}
//		this.target.transform.localPosition += new Vector3(0f, 20f, 0f);
//		this.target.transform.localScale = new Vector3(this.nextFloorScale, 1f, this.nextFloorScale);
//		this.lastFloorScale = this.nextFloorScale;
//		base.StartCoroutine("StartTargetPointer");
//		this.target.transform.DOLocalMoveY(this.target.transform.localPosition.y - 5f, 0.1f, false).SetEase(Ease.OutBounce).OnComplete(new TweenCallback(this.TargetMove2));
//		base.StartCoroutine("FloorSound");
//		if (!this.tutorialActive && (GameController.gameMode == 1 || GameController.gameMode == 4))
//		{
//			base.StartCoroutine("CheckAndSpawnGem");
//		}
//	}
//
//	// Token: 0x0600004B RID: 75 RVA: 0x000040EC File Offset: 0x000022EC
//	private IEnumerator FloorSound()
//	{
//		yield return new WaitForSeconds(0.2f);
//		this.soundC.PlaySound("land2");
//		yield return new WaitForSeconds(0.125f);
//		this.soundC.PlaySound("land2");
//		yield return new WaitForSeconds(0.125f);
//		this.soundC.PlaySound("land2");
//		yield break;
//	}
//
//	// Token: 0x0600004C RID: 76 RVA: 0x00004108 File Offset: 0x00002308
//	private void TargetMove2()
//	{
//		this.target.transform.DOLocalMoveY(this.target.transform.localPosition.y - 15f, 0.35f, false).SetEase(Ease.OutBounce);
//	}
//
//	// Token: 0x0600004D RID: 77 RVA: 0x00004154 File Offset: 0x00002354
//	private IEnumerator StartTargetPointer()
//	{
//		this.targetpointer.SetActive(false);
//		yield return 0;
//		if (GameController.perfectHitCounter > 0)
//		{
//			yield return new WaitForSeconds(0.35f);
//			this.targetpointer.SetActive(true);
//			this.targetpointer.transform.parent = this.target.transform;
//			this.targetpointer.transform.localPosition = new Vector3(0f, 0.21f, 0f);
//			this.targetpointer.transform.localScale = Vector3.zero;
//			this.targetpointer.transform.DOScale(1.2f / this.target.transform.localScale.x, 0.15f).SetId("targetpointer");
//		}
//		yield break;
//	}
//
//	// Token: 0x0600004E RID: 78 RVA: 0x00004170 File Offset: 0x00002370
//	private IEnumerator CheckAndSpawnGem()
//	{
//		yield return new WaitForSeconds(0.75f);
//		if (UnityEngine.Random.Range(0, 5) == 0 && this.score > 0)
//		{
//			this.tempGem = PoolManager.Pools["MainPool"].Spawn(this.gemPrefab.transform, new Vector3(this.target.transform.position.x, 1f, this.target.transform.position.z), Quaternion.identity);
//			this.tempGem.transform.eulerAngles = Vector3.zero;
//			this.tempGem.localScale = Vector3.zero;
//			this.tempGemParent = this.target.transform;
//			this.tempGem.DOScale(1f, 0.3f).SetEase(Ease.OutBounce).OnComplete(new TweenCallback(this.ParentGem));
//		}
//		yield break;
//	}
//
//	// Token: 0x0600004F RID: 79 RVA: 0x0000418C File Offset: 0x0000238C
//	private IEnumerator CheckAndSpawnStackingGem()
//	{
//		Vector3 tmpPos = this.player.transform.position;
//		yield return new WaitForSeconds(0.2f);
//		if (UnityEngine.Random.Range(0, 3) == 0)
//		{
//			this.tempGem = PoolManager.Pools["MainPool"].Spawn(this.gemPrefab.transform, new Vector3(tmpPos.x, tmpPos.y + 3.08f, tmpPos.z), Quaternion.identity);
//			this.tempGem.transform.eulerAngles = Vector3.zero;
//			this.tempGem.localScale = Vector3.zero;
//			this.tempGem.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
//		}
//		yield break;
//	}
//
//	// Token: 0x06000050 RID: 80 RVA: 0x000041A8 File Offset: 0x000023A8
//	private void ParentGem()
//	{
//		this.tempGem.transform.parent = this.tempGemParent;
//	}
//
//	// Token: 0x06000051 RID: 81 RVA: 0x000041C0 File Offset: 0x000023C0
//	public void SpawnThreeFastClickFloors()
//	{
//		this.fastFloorsActive = true;
//		this.floorList.Add(PoolManager.Pools["MainPool"].Spawn(this.fastFloor, new Vector3(this.spawner.transform.position.x, this.spawner.transform.position.y - 8f, this.spawner.transform.position.z), Quaternion.identity));
//		this.floorList.Add(PoolManager.Pools["MainPool"].Spawn(this.fastFloor, new Vector3(this.spawner.transform.position.x, this.spawner.transform.position.y - 8f, this.spawner.transform.position.z + 5f), Quaternion.identity));
//		this.floorList.Add(PoolManager.Pools["MainPool"].Spawn(this.fastFloor, new Vector3(this.spawner.transform.position.x, this.spawner.transform.position.y - 8f, this.spawner.transform.position.z + 10f), Quaternion.identity));
//		this.target = this.floorList[0].gameObject;
//		this.spawner.transform.position = new Vector3(this.spawner.transform.position.z, this.spawner.transform.position.y, this.spawner.transform.position.z + 10f);
//		this.lastFloorScale = 1.5f;
//	}
//
//	// Token: 0x06000052 RID: 82 RVA: 0x000043E4 File Offset: 0x000025E4
//	public void CycleThroughFastClickFloors()
//	{
//		this.target = this.floorList[this.activeFastFloor].gameObject;
//	}
//
//	// Token: 0x06000053 RID: 83 RVA: 0x00004404 File Offset: 0x00002604
//	public void raiseActiveFastFloorNumber()
//	{
//		this.activeFastFloor++;
//	}
//
//	// Token: 0x06000054 RID: 84 RVA: 0x00004414 File Offset: 0x00002614
//	private IEnumerator ChargeUp()
//	{
//		if (GameController.gameMode == 2)
//		{
//			this.player.GetComponent<TrailRenderer>().enabled = true;
//		}
//		this.soundC.PlaySound("charge");
//		this.player.transform.DOKill(false);
//		this.player.transform.localScale = Vector3.one;
//		this.player.transform.DOScale(new Vector3(1.75f, 0.5f, 1.75f), 1.5f).SetId("ScaleUp").SetEase(Ease.Linear);
//		this.activeFloor.transform.DOKill(false);
//		if (!GameController.bonusLevelActive)
//		{
//			if (GameController.gameMode != 2)
//			{
//				this.activeFloor.transform.DOLocalMoveY(-1.66f, 1.5f, false).SetId("MoveDownFloor").SetEase(Ease.Linear);
//				this.player.transform.DOLocalMoveY(-1.08f, 1.5f, false).SetId("MoveDown").SetEase(Ease.Linear);
//			}
//			else
//			{
//				this.activeFloor.transform.DOLocalMoveY(-1.66f + (float)this.score * 2.66f, 1.5f, false).SetId("MoveDownFloor").SetEase(Ease.Linear);
//				this.player.transform.DOLocalMoveY(-1.08f + (float)this.score * 2.66f, 1.5f, false).SetId("MoveDown").SetEase(Ease.Linear);
//			}
//			this.activeFloor.transform.DOScaleY(0.5f, 1.5f).SetId("ScaleFloor").SetEase(Ease.Linear);
//		}
//		else
//		{
//			this.activeFloor.transform.DOLocalMoveY(-10f, 1.5f, false).SetId("MoveDown").SetEase(Ease.Linear);
//			this.player.transform.DOLocalMoveY(-2f, 1.5f, false).SetId("MoveDown").SetEase(Ease.Linear);
//		}
//		this.chargeParticles.SetActive(true);
//		if (this.tutorialActive)
//		{
//			while (this.jumpPower < Vector3.Distance(this.player.transform.position, new Vector3(this.target.transform.position.x, this.player.transform.position.y, this.target.transform.position.z)))
//			{
//				this.jumpPower += 12.5f * Time.deltaTime;
//				yield return 0;
//			}
//			this.jumpPower = Vector3.Distance(this.player.transform.position, new Vector3(this.target.transform.position.x, this.player.transform.position.y, this.target.transform.position.z));
//			this.StartJumpAction();
//			this.TutorialFingerMoveBack();
//			yield break;
//		}
//		for (;;)
//		{
//			this.jumpPower += 12.5f * Time.deltaTime;
//			yield return 0;
//		}
//	}
//
//	// Token: 0x06000055 RID: 85 RVA: 0x00004430 File Offset: 0x00002630
//	public void GameOver()
//	{
//		HeyzapAds.ResumeExpensiveWork();
//		this.soundC.PlaySound("gameOver");
//		this.gameOver = true;
//		this.HideScoreText();
//		this.HideBowlingRoundCounter();
//		base.StartCoroutine("GameOverCoroutine");
//		this.adController.ShowBanner(true);
//		if (GameController.gameMode == 4)
//		{
//			base.StopCoroutine("ChargeUp");
//			this.chargeParticles.SetActive(false);
//			this.playerC.SetGravityActive();
//			DOTween.Kill("ScaleUp", false);
//			DOTween.Kill("MoveDown", false);
//			DOTween.Kill("MoveDownFloor", false);
//			this.player.transform.DOScale(1f, 0.5f);
//			this.jumpPower = 0f;
//			this.soundC.StopCharging();
//			this.didCharge = false;
//		}
//	}
//
//	// Token: 0x06000056 RID: 86 RVA: 0x00004508 File Offset: 0x00002708
//	private IEnumerator GameOverCoroutine()
//	{
//		yield return new WaitForSeconds(0.3f);
//		if (!this.showingPopup)
//		{
//			this.adController.CheckAndShowAds();
//		}
//		else
//		{
//			this.showingPopup = false;
//		}
//		yield return new WaitForSeconds(0.2f);
//		base.StartCoroutine("SaveVariables");
//		base.StartCoroutine("ShowGameOverScreen");
//		yield return new WaitForSeconds(0.3f);
//		this.adController.StartCoroutine("CheckVideoAds");
//		yield break;
//	}
//
//	// Token: 0x06000057 RID: 87 RVA: 0x00004524 File Offset: 0x00002724
//	public void ResetGame()
//	{
//		this.camC.transform.position = new Vector3(-17f, 17f, -17f);
//		if (GameController.gameMode == 3)
//		{
//			this.bowlingCounter = 0;
//		}
//		else if (GameController.gameMode == 2)
//		{
//			this.bgFloor.transform.parent = Camera.main.transform;
//			this.bgFloor.transform.localPosition = new Vector3(0f, 0f, 35.3f);
//			GameObject[] array = GameObject.FindGameObjectsWithTag("StackingBottle");
//			for (int i = 0; i < array.Length; i++)
//			{
//				UnityEngine.Object.Destroy(array[i]);
//			}
//			this.playerC.EquipCharacter(UnityEngine.Object.Instantiate<GameObject>(this.stackingBottle));
//		}
//		this.adController.StopCoroutine("CheckVideoAds");
//		DOTween.KillAll(false);
//		PoolManager.Pools["MainPool"].DespawnAll();
//		base.StartCoroutine("ReadMenuPixelColor");
//		this.floor2BGMat.color = this.floorColors2[UnityEngine.Random.Range(0, this.floorColors2.Length)];
//		this.floorList.Clear();
//		this.activeFloorMat = this.floorMats[0];
//		this.savedMaxJumpPower = 0f;
//		this.gameStarted = false;
//		this.spawner.transform.position = Vector3.zero;
//		base.StartCoroutine("DisableTrailRenderer");
//		this.playerC.ResetPlayer();
//		this.floorsCounter = 0;
//		this.lastFloorScale = 1.5f;
//		this.score = 0;
//		this.scoreText.text = this.score.ToString();
//		this.SpawnFirstFloor();
//		this.RandomizeNextFloorPosition();
//		this.SpawnFloor(false);
//		this.activeFloor = this.firstFloor;
//		if (GameController.gameMode != 2)
//		{
//			this.camC.MoveCamera();
//		}
//		this.isJumping = false;
//		GameController.perfectHitCounter = 0;
//		this.floorColorChangeChecker = 0;
//		this.startGameCollider.SetActive(true);
//		base.StartCoroutine("ShowMainMenu");
//		base.StartCoroutine("ScalePlayer");
//		if (this.videoAdButton.activeInHierarchy)
//		{
//			this.HideVideoAdButton();
//		}
//		for (int j = 0; j < this.carpets.Length; j++)
//		{
//			this.carpets[j].transform.position = this.carpetPos[j];
//		}
//		this.savedBowlingScore = 0;
//	}
//
//	// Token: 0x06000058 RID: 88 RVA: 0x000047A4 File Offset: 0x000029A4
//	public void ResetGameAfterTutorial()
//	{
//		this.soundC.PlaySound("buttonSound");
//		base.StopCoroutine("ChargeUp");
//		this.chargeParticles.SetActive(false);
//		this.soundC.StopCharging();
//		this.didCharge = false;
//		DOTween.Kill("ScaleUp", false);
//		DOTween.Kill("MoveDown", false);
//		this.player.transform.localScale = Vector3.one;
//		this.gameOver = true;
//		CryptoPlayerPrefs.SetInt("ShowTutorial", 0);
//		this.tutorialActive = false;
//		DOTween.KillAll(false);
//		base.StopAllCoroutines();
//		this.tutorialGO.SetActive(false);
//		this.tutorialActive = false;
//		this.jumpPower = 0f;
//		this.ResetGame();
//		this.ShowMenuGemCount();
//	}
//
//	// Token: 0x06000059 RID: 89 RVA: 0x00004868 File Offset: 0x00002A68
//	private IEnumerator DisableTrailRenderer()
//	{
//		this.player.GetComponent<TrailRenderer>().enabled = false;
//		yield return new WaitForSeconds(1f);
//		this.player.GetComponent<TrailRenderer>().enabled = true;
//		yield break;
//	}
//
//	// Token: 0x0600005A RID: 90 RVA: 0x00004884 File Offset: 0x00002A84
//	private void SpawnFirstFloor()
//	{
//		if (GameController.gameMode == 1)
//		{
//			this.activeFloor = PoolManager.Pools["MainPool"].Spawn(this.floorArray[UnityEngine.Random.Range(0, this.floorArray.Length)], new Vector3(0f, -0.1f, 0f), Quaternion.identity).gameObject;
//			this.activeFloor.transform.localScale = new Vector3(1.25f, 1f, 1.25f);
//		}
//		else if (GameController.gameMode == 2)
//		{
//			this.activeFloor = PoolManager.Pools["MainPool"].Spawn(this.stackTable, new Vector3(0f, -0.1f, 0f), Quaternion.identity).gameObject;
//			this.activeFloor.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
//		}
//		else if (GameController.gameMode == 3)
//		{
//			this.activeFloor = PoolManager.Pools["MainPool"].Spawn(this.bowlingTable, new Vector3(0f, -0.1f, 0f), Quaternion.identity).gameObject;
//			this.activeFloor.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
//		}
//		else if (GameController.gameMode == 4)
//		{
//			this.activeFloor = PoolManager.Pools["MainPool"].Spawn(this.speedFloor, new Vector3(0f, -0.1f, 0f), Quaternion.identity).gameObject;
//			this.activeFloor.transform.localScale = new Vector3(1.25f, 1f, 1.25f);
//		}
//		this.firstFloor = this.activeFloor;
//		this.playerC.SetActiveFloor(this.activeFloor);
//	}
//
//	// Token: 0x0600005B RID: 91 RVA: 0x00004A80 File Offset: 0x00002C80
//	private IEnumerator ScalePlayer()
//	{
//		this.player.transform.position += new Vector3(0f, 10f, 0f);
//		this.isJumping = true;
//		yield return new WaitForSeconds(0.2f);
//		this.player.transform.DOLocalMoveY(this.player.transform.localPosition.y - 10f, 0.5f, false).SetEase(Ease.OutBounce);
//		this.player.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce).OnComplete(new TweenCallback(this.CanStartGame));
//		yield break;
//	}
//
//	// Token: 0x0600005C RID: 92 RVA: 0x00004A9C File Offset: 0x00002C9C
//	private void CanStartGame()
//	{
//		this.isJumping = false;
//	}
//
//	// Token: 0x0600005D RID: 93 RVA: 0x00004AA8 File Offset: 0x00002CA8
//	public void UpdateScore(int i, bool addToColorChecker)
//	{
//		if (this.fastJumpCounterActive)
//		{
//			this.score += i * 2;
//			if (i == 1)
//			{
//				this.ShowFastLabel();
//				this.soundC.PlaySound("fast");
//			}
//		}
//		else if (GameController.gameMode == 1 || GameController.gameMode == 4 || GameController.gameMode == 2)
//		{
//			this.score += i;
//		}
//		else if (GameController.gameMode == 3)
//		{
//			if (i == 10)
//			{
//				this.soundC.PlaySound("strike");
//			}
//			else if (i > 0)
//			{
//				this.soundC.PlaySound("positive");
//			}
//			if (this.bowlingCounter == 1)
//			{
//				this.score += i;
//			}
//			else
//			{
//				this.score += i - this.score;
//				this.score += this.savedBowlingScore;
//			}
//		}
//		if (!this.tutorialActive)
//		{
//			this.scoreText.text = this.score.ToString();
//			if (addToColorChecker)
//			{
//				this.floorColorChangeChecker++;
//			}
//			if (this.floorColorChangeChecker >= 10)
//			{
//				this.ChangeFloorColor2();
//				this.floorColorChangeChecker -= 10;
//			}
//		}
//	}
//
//	// Token: 0x0600005E RID: 94 RVA: 0x00004C3C File Offset: 0x00002E3C
//	public void GemCollected(int amount)
//	{
//		this.gemCount += amount;
//	}
//
//	// Token: 0x0600005F RID: 95 RVA: 0x00004C58 File Offset: 0x00002E58
//	public void StackingImpactBounce(GameObject toBounce)
//	{
//		this.stackingToBounce = toBounce;
//		this.stackingToBounce.transform.DOLocalMoveY(this.stackingToBounce.transform.localPosition.y - 0.2f, 0.05f, false).OnComplete(new TweenCallback(this.ResetStackingPosition));
//	}
//
//	// Token: 0x06000060 RID: 96 RVA: 0x00004CB4 File Offset: 0x00002EB4
//	public void ResetStackingPosition()
//	{
//		this.stackingToBounce.transform.DOLocalMoveY(this.stackingToBounce.transform.localPosition.y + 0.2f, 0.05f, false);
//	}
//
//	// Token: 0x06000061 RID: 97 RVA: 0x00004CF8 File Offset: 0x00002EF8
//	public void ImpactBounce()
//	{
//		this.player.transform.DOLocalMoveY(0.2f, 0.05f, false).OnComplete(new TweenCallback(this.ResetPosition));
//	}
//
//	// Token: 0x06000062 RID: 98 RVA: 0x00004D28 File Offset: 0x00002F28
//	public void ResetPosition()
//	{
//		this.player.transform.DOLocalMoveY(0.5f, 0.05f, false);
//	}
//
//	// Token: 0x06000063 RID: 99 RVA: 0x00004D48 File Offset: 0x00002F48
//	private void ShowFastLabel()
//	{
//		this.fastLabel.transform.gameObject.SetActive(true);
//		this.fastLabel.DOKill(false);
//		this.fastLabel.color = new Color(this.fastLabel.color.r, this.fastLabel.color.g, this.fastLabel.color.b, 1f);
//		this.fastLabel.DOFade(1f, 0.2f);
//		this.fastLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//		this.fastLabel.transform.DOScale(0.8f, 0.4f).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.FadeFastLabel));
//	}
//
//	// Token: 0x06000064 RID: 100 RVA: 0x00004E34 File Offset: 0x00003034
//	private void FadeFastLabel()
//	{
//		this.fastLabel.DOFade(0f, 0.8f).OnComplete(new TweenCallback(this.DisableFastLabel));
//	}
//
//	// Token: 0x06000065 RID: 101 RVA: 0x00004E60 File Offset: 0x00003060
//	private void DisableFastLabel()
//	{
//		this.fastLabel.transform.gameObject.SetActive(false);
//	}
//
//	// Token: 0x06000066 RID: 102 RVA: 0x00004E78 File Offset: 0x00003078
//	public void PerfectHit()
//	{
//		if (!this.tutorialActive)
//		{
//			float num = Vector3.Distance(new Vector3(this.player.transform.position.x, 0f, this.player.transform.position.z), new Vector3(this.activeFloor.transform.position.x, 0f, this.activeFloor.transform.position.z));
//			if (num < 0.4f)
//			{
//				GameController.perfectHitCounter++;
//				this.UpdateScore(GameController.perfectHitCounter, false);
//				Transform transform = PoolManager.Pools["MainPool"].Spawn(this.plusX.transform, this.player.transform.position, Quaternion.Euler(new Vector3(35f, 45f, 0f)));
//				if (GameController.perfectHitCounter < 10)
//				{
//					transform.localScale = new Vector3(1f + 0.1f * (float)GameController.perfectHitCounter, 1f + 0.1f * (float)GameController.perfectHitCounter, 1f + 0.1f * (float)GameController.perfectHitCounter);
//				}
//				else
//				{
//					transform.localScale = new Vector3(2f, 2f, 2f);
//				}
//				if (this.fastJumpCounterActive)
//				{
//					transform.GetComponent<PlusOne>().SetText(((GameController.perfectHitCounter + 1) * 2).ToString());
//				}
//				else
//				{
//					transform.GetComponent<PlusOne>().SetText((GameController.perfectHitCounter + 1).ToString());
//				}
//				this.perfectLabel.transform.gameObject.SetActive(true);
//				this.perfectLabel.DOKill(false);
//				this.perfectLabel.color = new Color(this.perfectLabel.color.r, this.perfectLabel.color.g, this.perfectLabel.color.b, 1f);
//				this.perfectLabel.DOFade(1f, 0.2f);
//				this.perfectLabel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
//				this.perfectLabel.transform.DOScale(0.8f, 0.4f).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.FadePerfectLabel));
//				PoolManager.Pools["MainPool"].Spawn(this.perfectHitSquare.transform, this.player.transform.position - new Vector3(0f, 0.2f, 0f), Quaternion.identity);
//				if (GameController.perfectHitCounter <= 3)
//				{
//					for (int i = 1; i < GameController.perfectHitCounter; i++)
//					{
//						base.StartCoroutine(this.DelayedPerfectHitSquare(this.player.transform.position, i));
//					}
//				}
//				else if (GameController.perfectHitCounter > 3)
//				{
//					for (int j = 1; j < 4; j++)
//					{
//						base.StartCoroutine(this.DelayedPerfectHitSquare(this.player.transform.position, j));
//					}
//				}
//				this.soundC.PlaySound("perfect", GameController.perfectHitCounter - 1);
//			}
//			else
//			{
//				GameController.perfectHitCounter = 0;
//				Transform transform2 = PoolManager.Pools["MainPool"].Spawn(this.plusOne.transform, this.player.transform.position, Quaternion.Euler(new Vector3(35f, 45f, 0f)));
//				if (this.fastJumpCounterActive)
//				{
//					transform2.GetComponent<PlusOne>().SetText(((GameController.perfectHitCounter + 1) * 2).ToString());
//				}
//				else
//				{
//					transform2.GetComponent<PlusOne>().SetText((GameController.perfectHitCounter + 1).ToString());
//				}
//			}
//		}
//	}
//
//	// Token: 0x06000067 RID: 103 RVA: 0x00005294 File Offset: 0x00003494
//	private IEnumerator DelayedPerfectHitSquare(Vector3 spawnP, int waitTime)
//	{
//		yield return new WaitForSeconds(0.12f * (float)waitTime);
//		PoolManager.Pools["MainPool"].Spawn(this.perfectHitSquare.transform, spawnP - new Vector3(0f, 0.2f, 0f), Quaternion.identity);
//		yield break;
//	}
//
//	// Token: 0x06000068 RID: 104 RVA: 0x000052CC File Offset: 0x000034CC
//	private void FadePerfectLabel()
//	{
//		this.perfectLabel.DOFade(0f, 0.8f).OnComplete(new TweenCallback(this.DisablePerfectHit));
//	}
//
//	// Token: 0x06000069 RID: 105 RVA: 0x000052F8 File Offset: 0x000034F8
//	private void DisablePerfectHit()
//	{
//		this.perfectLabel.transform.gameObject.SetActive(false);
//	}
//
//	// Token: 0x0600006A RID: 106 RVA: 0x00005310 File Offset: 0x00003510
//	private void ChangeFloorColor2()
//	{
//		this.floor2BGMat.DOColor(this.floorColors2[UnityEngine.Random.Range(0, this.floorColors2.Length)], 2f);
//	}
//
//	// Token: 0x0600006B RID: 107 RVA: 0x00005344 File Offset: 0x00003544
//	private void ChangeFloorColor()
//	{
//		this.activeFloorColor = UnityEngine.Random.Range(0, this.floorMats.Length);
//		while (this.activeFloorColor == this.lastFloorColor)
//		{
//			this.activeFloorColor = UnityEngine.Random.Range(0, this.floorMats.Length);
//		}
//		this.activeFloorMat = this.floorMats[this.activeFloorColor];
//		this.lastFloorColor = this.activeFloorColor;
//		this.colorChangeActive = true;
//		switch (this.activeFloorColor + 1)
//		{
//		case 1:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors1[UnityEngine.Random.Range(0, this.skyBoxColors1.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 2:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors2[UnityEngine.Random.Range(0, this.skyBoxColors2.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 3:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors3[UnityEngine.Random.Range(0, this.skyBoxColors3.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 4:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors4[UnityEngine.Random.Range(0, this.skyBoxColors4.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 5:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors5[UnityEngine.Random.Range(0, this.skyBoxColors5.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 6:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors6[UnityEngine.Random.Range(0, this.skyBoxColors6.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 7:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors7[UnityEngine.Random.Range(0, this.skyBoxColors7.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 8:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors8[UnityEngine.Random.Range(0, this.skyBoxColors8.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 9:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors9[UnityEngine.Random.Range(0, this.skyBoxColors9.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		case 10:
//			DOTween.To(() => this.tweeningColor, delegate(Color x)
//			{
//				this.tweeningColor = x;
//			}, this.skyBoxColors10[UnityEngine.Random.Range(0, this.skyBoxColors10.Length)], 5f).OnComplete(new TweenCallback(this.ColorChangeFinished)).SetId("ColorChange");
//			break;
//		}
//	}
//
//	// Token: 0x0600006C RID: 108 RVA: 0x000057C0 File Offset: 0x000039C0
//	private void ColorChangeFinished()
//	{
//		this.colorChangeActive = false;
//	}
//
//	// Token: 0x0600006D RID: 109 RVA: 0x000057CC File Offset: 0x000039CC
//	private IEnumerator RefreshSkyboxColor()
//	{
//		while (this.colorChangeActive)
//		{
//			this.skyBoxMat.SetColor("_Color2", this.tweeningColor);
//			yield return 0;
//		}
//		yield break;
//	}
//
//	// Token: 0x0600006E RID: 110 RVA: 0x000057E8 File Offset: 0x000039E8
//	private IEnumerator ShowMainMenu()
//	{
//		this.mainMenu.SetActive(true);
//		base.StopCoroutine("HideMainMenu");
//		this.titleText.transform.DOKill(false);
//		this.titleText.transform.DOLocalMoveY(11f, 0.5f, false).SetEase(Ease.OutQuint);
//		base.StartCoroutine("FadeTapTextCoroutine");
//		for (int i = 0; i < this.mainMenuBtns.Length; i++)
//		{
//			this.mainMenuBtns[i].transform.DOKill(false);
//			this.mainMenuBtns[i].transform.DOLocalMoveY(0f, 0.25f, false).SetEase(Ease.OutCirc);
//			yield return new WaitForSeconds(0.05f);
//		}
//		if (this.facebookBtn != null)
//		{
//			this.facebookBtn.SetActive(true);
//		}
//		yield return new WaitForSeconds(0.5f);
//		if (this.unlockedCharacters < 51 && this.gemCount >= this.cheapestPrice)
//		{
//			base.StartCoroutine("AnimateShopSprite");
//		}
//		if (this.facebookBtn != null)
//		{
//			this.facebookBtnSprite.DOFade(1f, 0.5f);
//		}
//		yield break;
//	}
//
//	// Token: 0x0600006F RID: 111 RVA: 0x00005804 File Offset: 0x00003A04
//	private void HideMainMenu()
//	{
//		base.StopCoroutine("AnimateShopSprite");
//		this.shopSprite.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
//		this.shopExklamation.SetActive(false);
//		base.StopCoroutine("ShowMainMenu");
//		this.titleText.transform.DOKill(false);
//		this.titleText.transform.DOLocalMoveY(20f, 0.31f, false).OnComplete(new TweenCallback(this.SetMainMenuInactive));
//		if (this.facebookBtn != null)
//		{
//			this.facebookBtnSprite.DOFade(0f, 0.3f);
//		}
//		for (int i = 0; i < this.mainMenuBtns.Length; i++)
//		{
//			this.mainMenuBtns[i].transform.DOKill(false);
//			if (i != 0)
//			{
//				this.mainMenuBtns[i].transform.DOLocalMoveY(-10f, 0.3f, false);
//			}
//			else
//			{
//				this.mainMenuBtns[i].transform.DOLocalMoveY(-20f, 0.3f, false);
//			}
//		}
//		this.tapToPlayText.transform.DOKill(false);
//		DOTween.Kill("taptext", false);
//		DOTween.ToAlpha(() => this.tapToPlayText.color, delegate(Color x)
//		{
//			this.tapToPlayText.color = x;
//		}, 0f, 0.2f).OnComplete(new TweenCallback(this.SetTapTextInActive));
//	}
//
//	// Token: 0x06000070 RID: 112 RVA: 0x00005988 File Offset: 0x00003B88
//	private void SetMainMenuInactive()
//	{
//		this.mainMenu.SetActive(false);
//		if (this.facebookBtn != null)
//		{
//			this.facebookBtn.SetActive(false);
//		}
//	}
//
//	// Token: 0x06000071 RID: 113 RVA: 0x000059B4 File Offset: 0x00003BB4
//	private IEnumerator ShowGameOverScreen()
//	{
//		this.gameOverScreen.SetActive(true);
//		base.StartCoroutine("CheckChinese");
//		this.gameOverHighScoreLabel.text = this.score.ToString();
//		this.gameOverBestScoreLabel.text = this.bestScore.ToString();
//		this.roundsPlayedText.transform.localPosition = new Vector3(this.roundsPlayedText.transform.localPosition.x, -18f, this.roundsPlayedText.transform.localPosition.z);
//		if (GameController.gameMode == 1)
//		{
//			this.bonusLevelBar.SetActive(true);
//			float tempBonusCounter = (float)(this.bonusLevelCounter - this.score);
//			if (tempBonusCounter > this.bonusLevelCounterGoal)
//			{
//				tempBonusCounter = this.bonusLevelCounterGoal;
//			}
//			this.avatar.transform.localPosition = new Vector3(-6.9f + 13.9f * tempBonusCounter / this.bonusLevelCounterGoal, this.avatar.transform.localPosition.y, this.avatar.transform.localPosition.z);
//			if (this.activeAvatar != this.activeCharacter)
//			{
//				UnityEngine.Object.Destroy(this.bonusLevelBarAvatar.GetChild(0).gameObject);
//				GameObject tempAvatar = UnityEngine.Object.Instantiate(this.characters[this.activeCharacter], this.bonusLevelBarAvatar.transform.position - new Vector3(0f, 0.75f, 0f), Quaternion.identity) as GameObject;
//				try
//				{
//					tempAvatar.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
//					for (int u = 0; u < tempAvatar.transform.childCount; u++)
//					{
//						tempAvatar.transform.GetChild(u).GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
//					}
//				}
//				catch (Exception ex)
//				{
//					Exception e = ex;
//					string bla = e.ToString();
//				}
//				Component[] childComps = tempAvatar.GetComponentsInChildren<MeshRenderer>();
//				foreach (MeshRenderer mR in childComps)
//				{
//					mR.shadowCastingMode = ShadowCastingMode.Off;
//				}
//				tempAvatar.transform.parent = this.bonusLevelBarAvatar.transform;
//				tempAvatar.transform.localEulerAngles = new Vector3(17f, 117f, 336f);
//				tempAvatar.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
//				tempAvatar.transform.localPosition -= new Vector3(0f, 0f, 1f);
//				this.activeAvatar = this.activeCharacter;
//			}
//		}
//		else if (GameController.gameMode == 4)
//		{
//			this.bonusLevelBar.SetActive(false);
//		}
//		this.gameOverLabel.transform.DOLocalMoveY(12f, 0.5f, false).SetEase(Ease.OutQuint);
//		yield return new WaitForSeconds(0.1f);
//		this.scoreElements.transform.DOLocalMoveY(1f, 0.5f, false).SetEase(Ease.OutQuint);
//		yield return new WaitForSeconds(0.1f);
//		if (GameController.gameMode == 1)
//		{
//			this.bonusLevelBar.transform.DOLocalMoveY(0f, 0.5f, false).SetEase(Ease.OutQuint);
//			yield return new WaitForSeconds(0.1f);
//		}
//		if ((float)this.bonusLevelCounter >= this.bonusLevelCounterGoal && GameController.gameMode == 1)
//		{
//			this.bonusLevelBtn.transform.DOLocalMoveY(0f, 0.5f, false).SetEase(Ease.OutQuint);
//			this.bonusLevelSkipBtn.transform.DOLocalMoveY(0f, 0.5f, false).SetEase(Ease.OutQuint);
//			yield return new WaitForSeconds(0.1f);
//		}
//		else
//		{
//			for (int i = 0; i < this.gameOverScreenBtns.Length; i++)
//			{
//				this.gameOverScreenBtns[i].transform.DOLocalMoveY(0f, 0.5f, false).SetEase(Ease.OutQuint);
//				yield return new WaitForSeconds(0.1f);
//			}
//		}
//		if (GameController.gameMode == 1)
//		{
//			base.StartCoroutine("GiveXP");
//		}
//		this.roundsPlayedText.text = ScriptLocalization.Get("ROUNDS PLAYED") + this.roundsPlayed.ToString();
//		this.roundsPlayedText.transform.DOLocalMoveY(-11f, 0.5f, false).SetEase(Ease.OutQuint);
//		yield break;
//	}
//
//	// Token: 0x06000072 RID: 114 RVA: 0x000059D0 File Offset: 0x00003BD0
//	private void HideGameOverScreen()
//	{
//		base.StopCoroutine("ShowGameOverScreen");
//		base.StopCoroutine("GiveXP");
//		this.gameOverLabel.transform.DOLocalMoveY(20f, 0.3f, false).SetEase(Ease.OutQuint).OnComplete(new TweenCallback(this.SetGameOverScreenInactive));
//		this.scoreElements.transform.DOLocalMoveY(-30f, 0.3f, false).SetEase(Ease.OutQuint);
//		this.bonusLevelBar.transform.DOLocalMoveY(-20f, 0.3f, false).SetEase(Ease.OutQuint);
//		this.bonusLevelBtn.transform.DOLocalMoveY(-13f, 0.3f, false).SetEase(Ease.OutQuint);
//		this.bonusLevelSkipBtn.transform.DOLocalMoveY(-13f, 0.3f, false).SetEase(Ease.OutQuint);
//		for (int i = 0; i < this.gameOverScreenBtns.Length; i++)
//		{
//			this.gameOverScreenBtns[i].transform.DOKill(false);
//			this.gameOverScreenBtns[i].transform.DOLocalMoveY(-13f, 0.3f, false).SetEase(Ease.OutQuint);
//		}
//		this.roundsPlayedText.transform.DOKill(false);
//		this.roundsPlayedText.transform.DOLocalMoveY(-18f, 0.5f, false).SetEase(Ease.OutQuint);
//		this.bonusGameParticles.SetActive(false);
//	}
//
//	// Token: 0x06000073 RID: 115 RVA: 0x00005B48 File Offset: 0x00003D48
//	private void SetGameOverScreenInactive()
//	{
//		this.gameOverScreen.SetActive(false);
//	}
//
//	// Token: 0x06000074 RID: 116 RVA: 0x00005B58 File Offset: 0x00003D58
//	private IEnumerator GiveXP()
//	{
//		if ((float)this.bonusLevelCounter > this.bonusLevelCounterGoal)
//		{
//			this.bonusLevelCounter = (int)this.bonusLevelCounterGoal;
//		}
//		yield return new WaitForSeconds(0.3f);
//		if (this.score > 0)
//		{
//			this.soundC.PlaySound("counter");
//		}
//		this.avatar.transform.DOLocalMoveX(-6.9f + 13.9f * (float)this.bonusLevelCounter / this.bonusLevelCounterGoal, 1.5f, false).SetEase(Ease.InOutSine).OnComplete(new TweenCallback(this.PlayBonusSound));
//		yield break;
//	}
//
//	// Token: 0x06000075 RID: 117 RVA: 0x00005B74 File Offset: 0x00003D74
//	private void PlayBonusSound()
//	{
//		if ((float)this.bonusLevelCounter == this.bonusLevelCounterGoal)
//		{
//			this.soundC.PlaySound("bonus");
//			this.bonusGameParticles.SetActive(true);
//		}
//	}
//
//	// Token: 0x06000076 RID: 118 RVA: 0x00005BB0 File Offset: 0x00003DB0
//	public void ResetFastJumpCounter()
//	{
//		this.fastJumpCounterActive = true;
//		this.fastJumpCounter = 1.5f;
//	}
//
//	// Token: 0x06000077 RID: 119 RVA: 0x00005BC4 File Offset: 0x00003DC4
//	private void PlayBonusLevel()
//	{
//		this.bonusLevelCounter = 0;
//		CryptoPlayerPrefs.SetInt("BonusLevelCounter", this.bonusLevelCounter);
//		this.RetryBtnPressed();
//	}
//
//	// Token: 0x06000078 RID: 120 RVA: 0x00005BE4 File Offset: 0x00003DE4
//	private void SkipBonusLevel()
//	{
//		this.bonusLevelCounter = 0;
//		CryptoPlayerPrefs.SetInt("BonusLevelCounter", this.bonusLevelCounter);
//		this.RetryBtnPressed();
//	}
//
//	// Token: 0x06000079 RID: 121 RVA: 0x00005C04 File Offset: 0x00003E04
//	private IEnumerator ReadPixelColor()
//	{
//		yield return new WaitForEndOfFrame();
//		this.texBG.ReadPixels(new Rect((float)(Screen.width - 1), (float)(Screen.height - 1), (float)Screen.width, (float)Screen.height), 0, 0);
//		this.texBG.Apply();
//		yield return 0;
//		this.btnBGColor = this.texBG.GetPixel(0, 0);
//		Color tmpColor = new Color(this.btnBGColor.r - 0.2f, this.btnBGColor.g - 0.2f, this.btnBGColor.b - 0.2f, 1f);
//		this.roundsPlayedText.color = tmpColor;
//		yield break;
//	}
//
//	// Token: 0x0600007A RID: 122 RVA: 0x00005C20 File Offset: 0x00003E20
//	private IEnumerator ReadMenuPixelColor()
//	{
//		yield return new WaitForEndOfFrame();
//		this.texBG.ReadPixels(new Rect((float)(Screen.width - 1), (float)(Screen.height - 1), (float)Screen.width, (float)Screen.height), 0, 0);
//		this.texBG.Apply();
//		yield return 0;
//		this.btnBGColor = this.texBG.GetPixel(0, 0);
//		yield break;
//	}
//
//	// Token: 0x0600007B RID: 123 RVA: 0x00005C3C File Offset: 0x00003E3C
//	private void RetryBtnPressed()
//	{
//		this.soundC.StopCounterSound();
//		this.soundC.PlaySound("buttonSound");
//		this.ResetGame();
//		this.HideGameOverScreen();
//		base.StartCoroutine("ShowMainMenu");
//		this.ShowMenuGemCount();
//	}
//
//	// Token: 0x0600007C RID: 124 RVA: 0x00005C84 File Offset: 0x00003E84
//	private IEnumerator FadeTapTextCoroutine()
//	{
//		this.tapToPlayText.color = new Color(this.tapToPlayText.color.r, this.tapToPlayText.color.g, this.tapToPlayText.color.b, 0f);
//		yield return new WaitForSeconds(0.2f);
//		this.tapToPlayText.transform.DOKill(false);
//		this.tapToPlayText.gameObject.SetActive(true);
//		this.loopTapText = true;
//		this.FadeTapTextIn();
//		yield break;
//	}
//
//	// Token: 0x0600007D RID: 125 RVA: 0x00005CA0 File Offset: 0x00003EA0
//	private void SetTapTextInActive()
//	{
//		this.tapToPlayText.gameObject.SetActive(false);
//	}
//
//	// Token: 0x0600007E RID: 126 RVA: 0x00005CB4 File Offset: 0x00003EB4
//	private void FadeTapTextIn()
//	{
//		if (this.loopTapText)
//		{
//			DOTween.Kill("taptext", false);
//			DOTween.ToAlpha(() => this.tapToPlayText.color, delegate(Color x)
//			{
//				this.tapToPlayText.color = x;
//			}, 1f, 1f).OnComplete(new TweenCallback(this.FadeTapTextOut)).SetId("taptext");
//		}
//	}
//
//	// Token: 0x0600007F RID: 127 RVA: 0x00005D1C File Offset: 0x00003F1C
//	private void FadeTapTextOut()
//	{
//		if (this.loopTapText)
//		{
//			DOTween.Kill("taptext", false);
//			DOTween.ToAlpha(() => this.tapToPlayText.color, delegate(Color x)
//			{
//				this.tapToPlayText.color = x;
//			}, 0f, 1f).OnComplete(new TweenCallback(this.FadeTapTextIn)).SetId("taptext");
//		}
//	}
//
//	// Token: 0x06000080 RID: 128 RVA: 0x00005D84 File Offset: 0x00003F84
//	private IEnumerator ShowScoreText()
//	{
//		yield return new WaitForSeconds(0.2f);
//		this.scoreText.transform.DOLocalMoveY(12f, 0.5f, false);
//		yield break;
//	}
//
//	// Token: 0x06000081 RID: 129 RVA: 0x00005DA0 File Offset: 0x00003FA0
//	public void HideScoreText()
//	{
//		this.scoreText.transform.DOLocalMoveY(18f, 0.2f, false);
//	}
//
//	// Token: 0x06000082 RID: 130 RVA: 0x00005DC0 File Offset: 0x00003FC0
//	private void ShowMenuGemCount()
//	{
//		this.menuGemCountContainer.SetActive(true);
//		this.menuGemCount.text = this.gemCount.ToString() + " X";
//		this.menuGemCountContainer.transform.DOKill(false);
//		this.menuGemCountContainer.transform.DOLocalMoveY(0f, 0.5f, false).SetEase(Ease.OutQuint);
//	}
//
//	// Token: 0x06000083 RID: 131 RVA: 0x00005E30 File Offset: 0x00004030
//	private void HideMenuGemCount()
//	{
//		this.menuGemCountContainer.transform.DOKill(false);
//		this.menuGemCountContainer.transform.DOLocalMoveY(2f, 0.3f, false).SetEase(Ease.OutQuint).OnComplete(new TweenCallback(this.DisableGemCount));
//	}
//
//	// Token: 0x06000084 RID: 132 RVA: 0x00005E84 File Offset: 0x00004084
//	private void DisableGemCount()
//	{
//		this.menuGemCountContainer.SetActive(false);
//	}
//
//	// Token: 0x06000085 RID: 133 RVA: 0x00005E94 File Offset: 0x00004094
//	public bool SoundOnOff()
//	{
//		if (this.volumeOn)
//		{
//			AudioListener.volume = 0f;
//			this.volumeOn = false;
//			CryptoPlayerPrefs.SetInt("Volume", 0);
//		}
//		else
//		{
//			AudioListener.volume = 1f;
//			this.volumeOn = true;
//			CryptoPlayerPrefs.SetInt("Volume", 1);
//		}
//		return this.volumeOn;
//	}
//
//	// Token: 0x06000086 RID: 134 RVA: 0x00005EF0 File Offset: 0x000040F0
//	private void ShowSettings()
//	{
//		if (this.activeShopGO == null && this.tempSettings == null)
//		{
//			this.loopTapText = false;
//			base.StopCoroutine("FadeTapTextCoroutine");
//			this.soundC.PlaySound("buttonSound");
//			base.StopCoroutine("ShowMainMenu");
//			this.HideMainMenu();
//			this.startGameCollider.SetActive(false);
//			this.tempSettings = (UnityEngine.Object.Instantiate(this.settingsGO, Vector3.zero, Quaternion.identity) as GameObject);
//			this.tempSettings.transform.parent = Camera.main.transform;
//			this.tempSettings.transform.localPosition = new Vector3(-16f, 0f, 1f);
//			this.tempSettings.transform.localEulerAngles = Vector3.zero;
//			this.tempSettings.transform.DOLocalMoveX(0f, 0.55f, false).SetEase(Ease.OutExpo);
//			this.HideMenuGemCount();
//		}
//	}
//
//	// Token: 0x06000087 RID: 135 RVA: 0x00005FFC File Offset: 0x000041FC
//	public void HideSettings()
//	{
//		if (GameController.gameMode == 2)
//		{
//			this.bgFloor.transform.parent = Camera.main.transform;
//			this.bgFloor.transform.localPosition = new Vector3(0f, 0f, 35.3f);
//		}
//		this.soundC.PlaySound("buttonSound");
//		base.StartCoroutine("ShowMainMenu");
//		this.tempSettings.transform.DOLocalMoveX(-20f, 0.55f, false).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.DestroySettings));
//		this.ShowMenuGemCount();
//	}
//
//	// Token: 0x06000088 RID: 136 RVA: 0x000060A8 File Offset: 0x000042A8
//	private void DestroySettings()
//	{
//		UnityEngine.Object.Destroy(this.tempSettings);
//		this.tempSettings = null;
//		this.startGameCollider.SetActive(true);
//	}
//
//	// Token: 0x06000089 RID: 137 RVA: 0x000060C8 File Offset: 0x000042C8
//	public void FacebookLike()
//	{
//		this.soundC.PlaySound("buttonSound");
//		if (this.facebookBtn != null && this.facebookBtn.activeInHierarchy)
//		{
//			UnityEngine.Object.Destroy(this.facebookBtn);
//		}
//		CryptoPlayerPrefs.SetInt("FacebookLike", 1);
//		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
//		@static.Call("FacebookAndroidLike", new object[0]);
//	}
//
//	// Token: 0x0600008A RID: 138 RVA: 0x00006144 File Offset: 0x00004344
//	public void FacebookLikeMauigo()
//	{
//		this.soundC.PlaySound("buttonSound");
//		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
//		@static.Call("FacebookAndroidLikeMauigo", new object[0]);
//	}
//
//	// Token: 0x0600008B RID: 139 RVA: 0x0000618C File Offset: 0x0000438C
//	public void TwitterFollow()
//	{
//		this.soundC.PlaySound("buttonSound");
//		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
//		@static.Call("TwitterFollow", new object[0]);
//	}
//
//	// Token: 0x0600008C RID: 140 RVA: 0x000061D4 File Offset: 0x000043D4
//	public void TwitterMauigoFollow()
//	{
//		this.soundC.PlaySound("buttonSound");
//		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
//		@static.Call("TwitterMauigoFollow", new object[0]);
//	}
//
//	// Token: 0x0600008D RID: 141 RVA: 0x0000621C File Offset: 0x0000441C
//	public void RateBtn()
//	{
//		if (this.gameOver)
//		{
//			this.soundC.PlaySound("buttonSound");
//			if (Application.platform == RuntimePlatform.Android)
//			{
//				Application.OpenURL("market://details?id=com.ketchapp.bottleflip");
//			}
//			else if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				Application.OpenURL("https://itunes.apple.com/app/bottle-flip/id1178454068");
//			}
//		}
//	}
//
//	// Token: 0x0600008E RID: 142 RVA: 0x00006274 File Offset: 0x00004474
//	public void ProVersion()
//	{
//		if (this.gameOver)
//		{
//			this.soundC.PlaySound("buttonSound");
//			if (SA_Singleton<UM_InAppPurchaseManager>.Instance.IsProductPurchased("pro_version"))
//			{
//				base.SendMessage("SetPro");
//				this.isPro = true;
//				CryptoPlayerPrefs.SetInt("IsPro", 1);
//			}
//			else
//			{
//				SA_Singleton<UM_InAppPurchaseManager>.Instance.Purchase("pro_version");
//			}
//		}
//	}
//
//	// Token: 0x0600008F RID: 143 RVA: 0x000062E8 File Offset: 0x000044E8
//	public void RestoreTutorial()
//	{
//		CryptoPlayerPrefs.SetInt("ShowTutorial", 1);
//		if (GameController.gameMode == 1)
//		{
//			this.tutorialActive = true;
//		}
//	}
//
//	// Token: 0x06000090 RID: 144 RVA: 0x00006308 File Offset: 0x00004508
//	public void ShowShop()
//	{
//		if (this.activeShopGO == null && this.tempSettings == null)
//		{
//			this.floor2MR.receiveShadows = false;
//			this.adController.StartCoroutine("CheckShopVideoAds");
//			this.soundC.PlaySound("buttonSound");
//			this.startGameCollider.SetActive(false);
//			this.activeShopGO = UnityEngine.Object.Instantiate<GameObject>(this.shopGO);
//			this.activeShopGO.transform.parent = Camera.main.transform;
//			this.activeShopGO.transform.localPosition = new Vector3(-30f, 0f, 0f);
//			this.activeShopGO.transform.localEulerAngles = Vector3.zero;
//			this.activeShopGO.transform.DOLocalMoveX(0f, 0.3f, false);
//			Camera.main.transform.DOLocalMove(new Vector3(-50f, 17f, -2f), 0.3f, false);
//			base.StartCoroutine("HideMainMenu");
//		}
//	}
//
//	// Token: 0x06000091 RID: 145 RVA: 0x00006428 File Offset: 0x00004628
//	public void BackFromShop(int unlockedColors, int price)
//	{
//		if (GameController.gameMode == 2)
//		{
//			this.bgFloor.transform.parent = Camera.main.transform;
//			this.bgFloor.transform.localPosition = new Vector3(0f, 0f, 35.3f);
//		}
//		this.floor2MR.receiveShadows = true;
//		this.cheapestPrice = price;
//		CryptoPlayerPrefs.SetInt("CheapestPrice", this.cheapestPrice);
//		this.unlockedCharacters = unlockedColors;
//		this.camC.MoveCamera(0.3f);
//		this.adController.StopCoroutine("CheckShopVideoAds");
//		this.soundC.PlaySound("buttonSound");
//		this.activeShopGO.transform.DOLocalMoveX(-30f, 0.3f, false).OnComplete(new TweenCallback(this.DestroyShop));
//		base.StartCoroutine("ShowMainMenu");
//		this.LoadVariables();
//		this.HideShopVideoAdButton();
//	}
//
//	// Token: 0x06000092 RID: 146 RVA: 0x00006520 File Offset: 0x00004720
//	private void DestroyShop()
//	{
//		this.startGameCollider.SetActive(true);
//		UnityEngine.Object.Destroy(this.activeShopGO);
//		this.activeShopGO = null;
//	}
//
//	// Token: 0x06000093 RID: 147 RVA: 0x00006540 File Offset: 0x00004740
//	public void ShowGameModeSelect()
//	{
//		if (this.activeShopGO == null && this.tempSettings == null && this.tempGameModeSelect == null && !this.gameStarted)
//		{
//			this.bgFloor.transform.parent = Camera.main.transform;
//			this.bgFloor.transform.localPosition = new Vector3(0f, 0f, 35.3f);
//			this.floor2MR.receiveShadows = false;
//			this.soundC.PlaySound("buttonSound");
//			this.startGameCollider.SetActive(false);
//			this.tempGameModeSelect = UnityEngine.Object.Instantiate<GameObject>(this.gameModeSelectGO);
//			this.tempGameModeSelect.transform.parent = Camera.main.transform;
//			this.tempGameModeSelect.transform.localPosition = new Vector3(-30f, 0f, 0f);
//			this.tempGameModeSelect.transform.localEulerAngles = Vector3.zero;
//			this.tempGameModeSelect.transform.DOLocalMoveX(0f, 0.3f, false);
//			Camera.main.transform.DOLocalMove(new Vector3(-50f, 17f, -2f), 0.3f, false);
//			base.StartCoroutine("HideMainMenu");
//			this.HideMenuGemCount();
//		}
//	}
//
//	// Token: 0x06000094 RID: 148 RVA: 0x000066B0 File Offset: 0x000048B0
//	public void BackFromGameModeSelect()
//	{
//		this.floor2MR.receiveShadows = true;
//		this.soundC.PlaySound("buttonSound");
//		this.tempGameModeSelect.transform.DOLocalMoveX(-30f, 0.3f, false).OnComplete(new TweenCallback(this.DestroyGameModeSelect));
//		base.StartCoroutine("ShowMainMenu");
//		this.LoadVariables();
//		this.ShowMenuGemCount();
//	}
//
//	// Token: 0x06000095 RID: 149 RVA: 0x00006720 File Offset: 0x00004920
//	private void DestroyGameModeSelect()
//	{
//		this.startGameCollider.SetActive(true);
//		UnityEngine.Object.Destroy(this.tempGameModeSelect);
//		this.tempGameModeSelect = null;
//	}
//
//	// Token: 0x06000096 RID: 150 RVA: 0x00006740 File Offset: 0x00004940
//	public void ResetAfterStrike()
//	{
//		this.bowlingCounter = 0;
//		this.bowlingRoundCounterText.text = "1/2";
//		DOTween.KillAll(false);
//		PoolManager.Pools["MainPool"].DespawnAll();
//		this.floorList.Clear();
//		this.savedMaxJumpPower = 0f;
//		this.spawner.transform.position = Vector3.zero;
//		base.StartCoroutine("DisableTrailRenderer");
//		this.playerC.ResetPlayer();
//		this.floorsCounter = 0;
//		this.lastFloorScale = 1.5f;
//		this.scoreText.text = this.score.ToString();
//		this.SpawnFirstFloor();
//		this.RandomizeNextFloorPosition();
//		this.SpawnFloor(false);
//		this.activeFloor = this.firstFloor;
//		this.camC.transform.position = new Vector3(-17f, 17f, -17f);
//		this.camC.MoveCamera();
//		this.isJumping = false;
//		base.StartCoroutine("ScalePlayer");
//		this.savedBowlingScore += 10;
//	}
//
//	// Token: 0x06000097 RID: 151 RVA: 0x0000685C File Offset: 0x00004A5C
//	public IEnumerator CheckChinese()
//	{
//		yield return 0;
//		if (LocalizationManager.CurrentLanguage.Equals("Chinese"))
//		{
//			GameObject[] tempLabels = GameObject.FindGameObjectsWithTag("TextLabel");
//			for (int i = 0; i < tempLabels.Length; i++)
//			{
//				tempLabels[i].GetComponent<TextMeshPro>().font = this.chineseFont;
//			}
//		}
//		yield break;
//	}
//
//	// Token: 0x06000098 RID: 152 RVA: 0x00006878 File Offset: 0x00004A78
//	public void ShareScore()
//	{
//		this.soundC.PlaySound("buttonSound");
//		base.StartCoroutine("TakeScreenshotAndShare");
//	}
//
//	// Token: 0x06000099 RID: 153 RVA: 0x00006898 File Offset: 0x00004A98
//	private IEnumerator TakeScreenshotAndShare()
//	{
//		yield return new WaitForEndOfFrame();
//		int width = Screen.width;
//		int height = Screen.height;
//		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
//		tex.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
//		tex.Apply();
//		int tempScore = this.score;
//		if (GameController.gameMode == 1)
//		{
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2"),
//					" https://itunes.apple.com/app/bottle-flip/id1178454068"
//				}), tex);
//			}
//			else if (Application.platform == RuntimePlatform.Android)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2"),
//					" https://play.google.com/store/apps/details?id=com.ketchapp.bottleflip"
//				}), tex);
//			}
//		}
//		else if (GameController.gameMode == 2)
//		{
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1_STACKING"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2_STACKING"),
//					" https://itunes.apple.com/app/bottle-flip/id1178454068"
//				}), tex);
//			}
//			else if (Application.platform == RuntimePlatform.Android)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1_STACKING"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2_STACKING"),
//					" https://play.google.com/store/apps/details?id=com.ketchapp.bottleflip"
//				}), tex);
//			}
//		}
//		else if (GameController.gameMode == 3)
//		{
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1_BOWLING"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2_BOWLING"),
//					" https://itunes.apple.com/app/bottle-flip/id1178454068"
//				}), tex);
//			}
//			else if (Application.platform == RuntimePlatform.Android)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1_BOWLING"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2_BOWLING"),
//					" https://play.google.com/store/apps/details?id=com.ketchapp.bottleflip"
//				}), tex);
//			}
//		}
//		else if (GameController.gameMode == 4)
//		{
//			if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1_SPEED"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2_SPEED"),
//					" https://itunes.apple.com/app/bottle-flip/id1178454068"
//				}), tex);
//			}
//			else if (Application.platform == RuntimePlatform.Android)
//			{
//				UM_ShareUtility.ShareMedia("Bottle Flip!", string.Concat(new string[]
//				{
//					ScriptLocalization.Get("SOCIALTEXT1_SPEED"),
//					" ",
//					this.score.ToString(),
//					" ",
//					ScriptLocalization.Get("SOCIALTEXT2_SPEED"),
//					" https://play.google.com/store/apps/details?id=com.ketchapp.bottleflip"
//				}), tex);
//			}
//		}
//		yield return 0;
//		yield break;
//	}
//
//	// Token: 0x0600009A RID: 154 RVA: 0x000068B4 File Offset: 0x00004AB4
//	private void VideoAdBtnPressed()
//	{
//		this.videoAdButton.transform.localPosition = new Vector3(14f, -5f, 2f);
//		this.adController.ShowVideoAdForCoins();
//	}
//
//	// Token: 0x0600009B RID: 155 RVA: 0x000068E8 File Offset: 0x00004AE8
//	private void ShopVideoAdBtnPressed()
//	{
//		this.shopVideoAdButton.transform.localPosition = new Vector3(14f, -9.2f, 2f);
//		this.adController.ShowShopVideoAdForCoins();
//	}
//
//	// Token: 0x0600009C RID: 156 RVA: 0x0000691C File Offset: 0x00004B1C
//	public void ShowVideoAdButton()
//	{
//		this.videoAdButton.SetActive(true);
//		this.videoAdButton.transform.DOLocalMoveX(6.1f, 0.5f, false);
//		base.StartCoroutine("CheckChinese");
//	}
//
//	// Token: 0x0600009D RID: 157 RVA: 0x00006960 File Offset: 0x00004B60
//	private void HideVideoAdButton()
//	{
//		this.videoAdButton.transform.DOKill(false);
//		this.videoAdButton.transform.DOLocalMoveX(14f, 0.1f, false).OnComplete(new TweenCallback(this.SetVideoAdButtonInactive));
//	}
//
//	// Token: 0x0600009E RID: 158 RVA: 0x000069AC File Offset: 0x00004BAC
//	private void SetVideoAdButtonInactive()
//	{
//		this.videoAdButton.SetActive(false);
//	}
//
//	// Token: 0x0600009F RID: 159 RVA: 0x000069BC File Offset: 0x00004BBC
//	public void ShowShopVideoAdButton()
//	{
//		this.shopVideoAdButton.SetActive(true);
//		this.shopVideoAdButton.transform.DOLocalMoveX(5.5f, 0.5f, false);
//		base.StartCoroutine("CheckChinese");
//	}
//
//	// Token: 0x060000A0 RID: 160 RVA: 0x00006A00 File Offset: 0x00004C00
//	private void HideShopVideoAdButton()
//	{
//		this.shopVideoAdButton.transform.DOLocalMoveX(14f, 0.1f, false).OnComplete(new TweenCallback(this.SetShopVideoAdButtonInactive));
//	}
//
//	// Token: 0x060000A1 RID: 161 RVA: 0x00006A30 File Offset: 0x00004C30
//	private void SetShopVideoAdButtonInactive()
//	{
//		this.shopVideoAdButton.SetActive(false);
//	}
//
//	// Token: 0x060000A2 RID: 162 RVA: 0x00006A40 File Offset: 0x00004C40
//	private void OnRatePopUpClose(MNDialogResult result)
//	{
//		switch (result)
//		{
//		case MNDialogResult.RATED:
//			CryptoPlayerPrefs.SetInt("ShowPopup", 0);
//			break;
//		case MNDialogResult.REMIND:
//			CryptoPlayerPrefs.SetInt("ShowPopup", 0);
//			break;
//		case MNDialogResult.DECLINED:
//			CryptoPlayerPrefs.SetInt("ShowPopup", 0);
//			break;
//		}
//	}
//
//	// Token: 0x060000A3 RID: 163 RVA: 0x00006A98 File Offset: 0x00004C98
//	public void AddIncentivizedGems(int newGems)
//	{
//		this.gemCount = CryptoPlayerPrefs.GetInt("GemCount", 0);
//		this.gemCount += newGems;
//		this.menuGemCount.text = this.gemCount.ToString() + " X";
//		CryptoPlayerPrefs.SetInt("GemCount", this.gemCount);
//		if (this.activeShopGO != null)
//		{
//		}
//	}
//
//	// Token: 0x060000A4 RID: 164 RVA: 0x00006B1C File Offset: 0x00004D1C
//	public void ActivateBonusLevel()
//	{
//		DOTween.KillAll(false);
//		if (this.videoAdButton.activeInHierarchy)
//		{
//			this.HideVideoAdButton();
//		}
//		for (int i = 0; i < this.carpets.Length; i++)
//		{
//			this.carpets[i].SetActive(false);
//		}
//		if (this.bonusGamesPlayed < 5)
//		{
//			this.bonusGamesPlayed++;
//			CryptoPlayerPrefs.SetInt("BonusGamesPlayed", this.bonusGamesPlayed);
//			this.SetBonusLevelGoal();
//		}
//		this.newFloorGO.SetActive(false);
//		this.soundC.PlaySound("buttonSound");
//		this.soundC.StopCounterSound();
//		this.bonusLevelCounter = 0;
//		CryptoPlayerPrefs.SetInt("BonusLevelCounter", this.bonusLevelCounter);
//		this.adController.StopCoroutine("CheckVideoAds");
//		PoolManager.Pools["MainPool"].DespawnAll();
//		this.floorList.Clear();
//		this.bonusBGParticles.SetActive(true);
//		this.colorChangeActive = false;
//		this.spawner.transform.position = Vector3.zero;
//		base.StartCoroutine("DisableTrailRenderer");
//		this.playerC.ResetPlayer();
//		this.score = 0;
//		this.scoreText.text = this.score.ToString();
//		this.isJumping = false;
//		GameController.perfectHitCounter = 0;
//		this.floorColorChangeChecker = 0;
//		base.StartCoroutine("ScalePlayer");
//		this.HideGameOverScreen();
//		GameController.bonusLevelActive = true;
//		this.playerC.SetBonusLevelActive(true);
//		this.SpawnBonusLevelFloor();
//		base.StartCoroutine("SpawnBonusLevelGem");
//		this.gameStarted = true;
//	}
//
//	// Token: 0x060000A5 RID: 165 RVA: 0x00006CBC File Offset: 0x00004EBC
//	private void SpawnBonusLevelFloor()
//	{
//		this.activeFloor = PoolManager.Pools["MainPool"].Spawn(this.bonusLevelFloor, new Vector3(0f, -8f, 0f), Quaternion.identity).gameObject;
//		Camera.main.transform.position = new Vector3(-17f, 17f, -17f);
//	}
//
//	// Token: 0x060000A6 RID: 166 RVA: 0x00006D2C File Offset: 0x00004F2C
//	public void SpawnNewBonusLevelGem()
//	{
//		float x = UnityEngine.Random.Range(-1f, 1f);
//		float z = UnityEngine.Random.Range(-1f, 1f);
//		Vector3 vector = new Vector3(x, 0f, z);
//		Vector3 a = vector.normalized;
//		a *= UnityEngine.Random.Range(0f, 7.5f);
//		while (Vector3.Distance(a, this.player.transform.position) < 3.5f)
//		{
//			x = UnityEngine.Random.Range(-1f, 1f);
//			z = UnityEngine.Random.Range(-1f, 1f);
//			Vector3 vector2 = new Vector3(x, 0f, z);
//			a = vector2.normalized;
//			a *= UnityEngine.Random.Range(0f, 7.5f);
//		}
//		this.bonusGemValue = UnityEngine.Random.Range(1, 11);
//		if (this.bonusGemValue <= 4)
//		{
//			this.bonusGemValue = 1;
//		}
//		else if (this.bonusGemValue <= 7)
//		{
//			this.bonusGemValue = 2;
//		}
//		else
//		{
//			this.bonusGemValue = 3;
//		}
//		this.tempGem = PoolManager.Pools["MainPool"].Spawn(this.bonusGems[this.bonusGemValue - 1].transform, Vector3.zero, Quaternion.identity);
//		this.tempGem.localScale = Vector3.zero;
//		this.target = this.tempGem.gameObject;
//		this.tempGemParent = this.activeFloor.transform;
//		float num = (float)this.bonusGemCounter;
//		if (num > 30f)
//		{
//			num = 30f;
//		}
//		this.tempGem.transform.position = new Vector3(a.x, 0.9f - num * 0.005f, a.z);
//		this.tempGem.DOScale(0.8f - num * 0.005f, 0.3f).SetEase(Ease.OutBounce).OnComplete(new TweenCallback(this.ParentGem));
//	}
//
//	// Token: 0x060000A7 RID: 167 RVA: 0x00006F24 File Offset: 0x00005124
//	private IEnumerator BonusGameCountDown()
//	{
//		int counter = 20;
//		this.scoreText.text = counter.ToString();
//		base.StartCoroutine("ShowScoreText");
//		while (counter > 0)
//		{
//			yield return new WaitForSeconds(1f);
//			counter--;
//			this.scoreText.text = counter.ToString();
//			if (counter == 4)
//			{
//				this.soundC.PlaySound("bonusCountDown");
//			}
//		}
//		base.StopCoroutine("ChargeUp");
//		this.chargeParticles.SetActive(false);
//		DOTween.Kill("ScaleUp", false);
//		DOTween.Kill("MoveDown", false);
//		DOTween.Kill("MoveDownFloor", false);
//		this.player.transform.DOScale(1f, 0.5f);
//		if (!this.isJumping)
//		{
//			this.player.transform.DOLocalMoveY(0.5f, 0.5f, false);
//		}
//		this.activeFloor.transform.DOMoveY(-8f, 0.35f, false).SetEase(Ease.OutBounce);
//		this.jumpPower = 0f;
//		this.soundC.StopCharging();
//		this.didCharge = false;
//		this.BonusGameOver();
//		yield break;
//	}
//
//	// Token: 0x060000A8 RID: 168 RVA: 0x00006F40 File Offset: 0x00005140
//	public void BonusGameOver()
//	{
//		if (!this.gameOver)
//		{
//			this.gameOver = true;
//			base.StopCoroutine("BonusGameCountDown");
//			this.HideScoreText();
//			this.soundC.StopBonusCountDown();
//			base.StartCoroutine("BonusGameOverCoroutine");
//		}
//	}
//
//	// Token: 0x060000A9 RID: 169 RVA: 0x00006F88 File Offset: 0x00005188
//	private IEnumerator BonusGameOverCoroutine()
//	{
//		yield return new WaitForSeconds(0.5f);
//		this.ShowMenuGemCount();
//		this.bonusGemsCollected.gameObject.SetActive(true);
//		this.bonusGemsCollected.text = "+" + this.bonusGemCounter.ToString();
//		this.bonusGemsCollected.transform.localPosition = new Vector3(0f, 8.8f, 1f);
//		this.bonusGemsCollected.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
//		this.bonusGemsCollected.transform.DOScale(1f, 0.6f).SetEase(Ease.OutExpo);
//		yield return new WaitForSeconds(1.2f);
//		this.MoveToGemCount();
//		yield return new WaitForSeconds(0.3f);
//		if (this.bonusGemCounter > 0)
//		{
//			this.soundC.PlaySound("bonus");
//		}
//		else
//		{
//			this.soundC.PlaySound("error");
//		}
//		this.gemCount += this.bonusGemCounter;
//		this.bonusGemCounter = 0;
//		this.menuGemCount.text = this.gemCount.ToString() + " X";
//		CryptoPlayerPrefs.SetInt("GemCount", this.gemCount);
//		yield return new WaitForSeconds(1.5f);
//		this.bonusBGParticles.SetActive(false);
//		base.StartCoroutine("ShowMainMenu");
//		GameController.bonusLevelActive = false;
//		this.playerC.SetBonusLevelActive(false);
//		for (int i = 0; i < this.carpets.Length; i++)
//		{
//			this.carpets[i].SetActive(true);
//		}
//		this.ResetGame();
//		this.soundC.PlaySound("buttonSound");
//		this.newFloorGO.SetActive(true);
//		yield break;
//	}
//
//	// Token: 0x060000AA RID: 170 RVA: 0x00006FA4 File Offset: 0x000051A4
//	private void MoveToGemCount()
//	{
//		this.bonusGemsCollected.transform.DOMove(this.menuGemCount.transform.position, 0.3f, false).OnComplete(new TweenCallback(this.DisableBonusGems));
//		this.bonusGemsCollected.transform.DOScale(0.01f, 0.3f);
//	}
//
//	// Token: 0x060000AB RID: 171 RVA: 0x00007004 File Offset: 0x00005204
//	private void DisableBonusGems()
//	{
//		this.bonusGemsCollected.gameObject.SetActive(false);
//	}
//
//	// Token: 0x060000AC RID: 172 RVA: 0x00007018 File Offset: 0x00005218
//	public IEnumerator SpawnBonusLevelGem()
//	{
//		this.soundC.PlaySound("countDown");
//		this.bonusGameCountDown.gameObject.SetActive(true);
//		this.bonusGameCountDown.text = "3";
//		this.bonusGameCountDown.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//		this.bonusGameCountDown.transform.DOScale(1f, 0.9f).SetEase(Ease.OutExpo);
//		yield return new WaitForSeconds(0.92f);
//		this.bonusGameCountDown.text = "2";
//		this.bonusGameCountDown.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//		this.bonusGameCountDown.transform.DOScale(1f, 0.9f).SetEase(Ease.OutExpo);
//		yield return new WaitForSeconds(0.92f);
//		this.bonusGameCountDown.text = "1";
//		this.bonusGameCountDown.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//		this.bonusGameCountDown.transform.DOScale(1f, 0.9f).SetEase(Ease.OutExpo);
//		yield return new WaitForSeconds(0.92f);
//		this.bonusGameCountDown.text = "GO";
//		this.bonusGameCountDown.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//		this.bonusGameCountDown.transform.DOScale(1f, 0.9f).SetEase(Ease.OutExpo).OnComplete(new TweenCallback(this.SetBonusGameCountdownInactive1));
//		yield return new WaitForSeconds(0.6f);
//		this.SpawnNewBonusLevelGem();
//		this.gameOver = false;
//		base.StartCoroutine("BonusGameCountDown");
//		yield break;
//	}
//
//	// Token: 0x060000AD RID: 173 RVA: 0x00007034 File Offset: 0x00005234
//	private void SetBonusGameCountdownInactive1()
//	{
//		this.bonusGameCountDown.transform.DOScale(0.05f, 0.2f).OnComplete(new TweenCallback(this.SetBonusGameCountdownInactive2));
//	}
//
//	// Token: 0x060000AE RID: 174 RVA: 0x00007070 File Offset: 0x00005270
//	private void SetBonusGameCountdownInactive2()
//	{
//		this.bonusGameCountDown.gameObject.SetActive(false);
//	}
//
//	// Token: 0x060000AF RID: 175 RVA: 0x00007084 File Offset: 0x00005284
//	public void IncreaseBonusGemCounter()
//	{
//		this.bonusGemCounter += this.bonusGemValue;
//	}
//
//	// Token: 0x060000B0 RID: 176 RVA: 0x0000709C File Offset: 0x0000529C
//	public void SpawnGemPlusXSprite(Vector3 gemSpawnPos)
//	{
//		PoolManager.Pools["MainPool"].Spawn(this.bonusGemParticles[this.bonusGemValue - 1].transform, gemSpawnPos, Quaternion.Euler(new Vector3(-90f, 0f, 0f)));
//		Transform transform = PoolManager.Pools["MainPool"].Spawn(this.bonusPlusX[this.bonusGemValue - 1].transform, this.player.transform.position, Quaternion.Euler(new Vector3(35f, 45f, 0f)));
//		transform.localScale = new Vector3(1f + 0.1f * (float)this.bonusGemValue, 1f + 0.1f * (float)this.bonusGemValue, 1f + 0.1f * (float)this.bonusGemValue);
//		transform.GetComponent<PlusOne>().SetText(this.bonusGemValue.ToString());
//	}
//
//	// Token: 0x060000B1 RID: 177 RVA: 0x00007198 File Offset: 0x00005398
//	private IEnumerator StartTutorial()
//	{
//		this.tutorialGO.SetActive(true);
//		base.StartCoroutine("CheckChinese");
//		this.tutorialGO.transform.localPosition = new Vector3(10f, 0f, 0f);
//		this.tutorialGO.transform.DOLocalMoveX(0f, 0.2f, false);
//		yield return new WaitForSeconds(0.2f);
//		this.TutorialFingerMove();
//		this.startButtonPressed = true;
//		yield break;
//	}
//
//	// Token: 0x060000B2 RID: 178 RVA: 0x000071B4 File Offset: 0x000053B4
//	private void TutorialFingerMove()
//	{
//		this.tutorialFinger.transform.DOLocalRotate(new Vector3(50f, 0f, 0f), 0.5f, RotateMode.FastBeyond360).OnComplete(new TweenCallback(this.TutorialChargeUp));
//		this.tutorialFingerShadow.transform.DOLocalMove(new Vector3(0f, 0.7f, 3.5f), 0.5f, false);
//		this.tutorialFingerShadow.transform.DOLocalRotate(new Vector3(50f, 0f, 0f), 0.5f, RotateMode.FastBeyond360);
//	}
//
//	// Token: 0x060000B3 RID: 179 RVA: 0x00007254 File Offset: 0x00005454
//	private void TutorialChargeUp()
//	{
//		base.StartCoroutine("ChargeUp");
//	}
//
//	// Token: 0x060000B4 RID: 180 RVA: 0x00007264 File Offset: 0x00005464
//	private void TutorialFingerMoveBack()
//	{
//		this.tutorialFinger.transform.DOLocalRotate(new Vector3(25f, 0f, 0f), 0.5f, RotateMode.FastBeyond360).OnComplete(new TweenCallback(this.WaitTutorialFingerMove));
//		this.tutorialFingerShadow.transform.DOLocalMove(new Vector3(-0.7f, -0.5f, 3.5f), 0.5f, false);
//		this.tutorialFingerShadow.transform.DOLocalRotate(new Vector3(25f, 0f, 0f), 0.5f, RotateMode.FastBeyond360);
//	}
//
//	// Token: 0x060000B5 RID: 181 RVA: 0x00007304 File Offset: 0x00005504
//	private void WaitTutorialFingerMove()
//	{
//		base.StartCoroutine("Waiting", 0.5f);
//	}
//
//	// Token: 0x060000B6 RID: 182 RVA: 0x0000731C File Offset: 0x0000551C
//	private IEnumerator Waiting(float waitTime)
//	{
//		yield return new WaitForSeconds(waitTime);
//		this.TutorialFingerMove();
//		yield break;
//	}
//
//	// Token: 0x060000B7 RID: 183 RVA: 0x00007348 File Offset: 0x00005548
//	private IEnumerator AnimateShopSprite()
//	{
//		this.shopExklamation.SetActive(true);
//		this.shopExklamation.transform.localScale = Vector3.zero;
//		this.shopExklamation.transform.DOScale(0.75f, 0.2f);
//		for (;;)
//		{
//			this.shopSprite.transform.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.5f).SetEase(Ease.OutBounce);
//			yield return new WaitForSeconds(0.5f);
//			this.shopSprite.transform.DOScale(new Vector3(0.7f, 0.7f, 1f), 0.2f);
//			yield return new WaitForSeconds(0.2f);
//		}
//		yield break;
//	}
//
//	// Token: 0x060000B8 RID: 184 RVA: 0x00007364 File Offset: 0x00005564
//	public void ShowLeaderboards()
//	{
//		if (this.gameOver)
//		{
//			this.soundC.PlaySound("buttonSound");
//			if (Application.platform == RuntimePlatform.Android)
//			{
//				if (GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED)
//				{
//					SA_Singleton<UM_GameServiceManager>.Instance.ShowLeaderBoardsUI();
//				}
//				else
//				{
//					SA_Singleton<GooglePlayConnection>.Instance.Connect();
//				}
//			}
//			else if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				SA_Singleton<UM_GameServiceManager>.Instance.ShowLeaderBoardsUI();
//			}
//		}
//	}
//
//	// Token: 0x060000B9 RID: 185 RVA: 0x000073D8 File Offset: 0x000055D8
//	public void ShowAchievements()
//	{
//		if (this.gameOver)
//		{
//			this.soundC.PlaySound("buttonSound");
//			if (Application.platform == RuntimePlatform.Android)
//			{
//				if (GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED)
//				{
//					SA_Singleton<GooglePlayManager>.Instance.ShowAchievementsUI();
//				}
//				else
//				{
//					SA_Singleton<GooglePlayConnection>.Instance.Connect();
//				}
//			}
//			else if (Application.platform == RuntimePlatform.IPhonePlayer)
//			{
//				GameCenterManager.ShowAchievements();
//			}
//		}
//	}
//
//	// Token: 0x060000BA RID: 186 RVA: 0x00007448 File Offset: 0x00005648
//	private void CheckAndSubmitAchievements()
//	{
//		if (GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED && Application.platform == RuntimePlatform.Android)
//		{
//			SA_Singleton<GooglePlayManager>.Instance.SubmitScore("bottleFlip_leaderboard", (long)this.score);
//			if (this.score >= 25)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievementLevel25");
//			}
//			if (this.score >= 50)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievementLevel50");
//			}
//			if (this.score >= 100)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievementLevel100");
//			}
//			if (this.score >= 250)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievementLevel250");
//			}
//			if (this.score >= 500)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievementLevel500");
//			}
//			if (this.score >= 1000)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievementLevel1000");
//			}
//			if (this.savedMaxJumpPower > 100f)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("bottleFlip_moon");
//			}
//		}
//		if (GameCenterManager.IsInitialized && Application.platform == RuntimePlatform.IPhonePlayer)
//		{
//			GameCenterManager.ReportScore((long)this.score, "bottleFlip_leaderboard");
//			if (this.score >= 25)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_points25", true);
//			}
//			if (this.score >= 50)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_points50", true);
//			}
//			if (this.score >= 100)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_points100", true);
//			}
//			if (this.score >= 250)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_points250", true);
//			}
//			if (this.score >= 500)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_points500", true);
//			}
//			if (this.score >= 1000)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_points1000", true);
//			}
//			if (this.savedMaxJumpPower > 100f)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_moon", true);
//			}
//		}
//	}
//
//	// Token: 0x060000BB RID: 187 RVA: 0x00007698 File Offset: 0x00005898
//	private void CheckAndSubmitAchievementsBowling()
//	{
//		if (GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED && Application.platform == RuntimePlatform.Android)
//		{
//			SA_Singleton<GooglePlayManager>.Instance.SubmitScore("bottleFlipBowling_leaderboard", (long)this.score);
//		}
//		if (GameCenterManager.IsInitialized && Application.platform == RuntimePlatform.IPhonePlayer)
//		{
//			GameCenterManager.ReportScore((long)this.score, "bottleFlipBowling_leaderboard");
//		}
//	}
//
//	// Token: 0x060000BC RID: 188 RVA: 0x00007704 File Offset: 0x00005904
//	private void CheckAndSubmitAchievementsSpeed()
//	{
//		if (GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED && Application.platform == RuntimePlatform.Android)
//		{
//			SA_Singleton<GooglePlayManager>.Instance.SubmitScore("bottleFlipSpeed_leaderboard", (long)this.score);
//		}
//		if (GameCenterManager.IsInitialized && Application.platform == RuntimePlatform.IPhonePlayer)
//		{
//			GameCenterManager.ReportScore((long)this.score, "bottleFlipSpeed_leaderboard");
//		}
//	}
//
//	// Token: 0x060000BD RID: 189 RVA: 0x00007770 File Offset: 0x00005970
//	private void CheckAndSubmitAchievementsStacking()
//	{
//		if (GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED && Application.platform == RuntimePlatform.Android)
//		{
//			SA_Singleton<GooglePlayManager>.Instance.SubmitScore("bottleFlipStacking_leaderboard", (long)this.score);
//		}
//		if (GameCenterManager.IsInitialized && Application.platform == RuntimePlatform.IPhonePlayer)
//		{
//			GameCenterManager.ReportScore((long)this.score, "bottleFlipStacking_leaderboard");
//		}
//	}
//
//	// Token: 0x060000BE RID: 190 RVA: 0x000077DC File Offset: 0x000059DC
//	private void CheckAndSubmitRoundsPlayed()
//	{
//		if (GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED && Application.platform == RuntimePlatform.Android)
//		{
//			if (this.roundsPlayed >= 100)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievement100Games");
//			}
//			if (this.roundsPlayed >= 500)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievement500Games");
//			}
//			if (this.roundsPlayed >= 1000)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievement1000Games");
//			}
//			if (this.roundsPlayed >= 5000)
//			{
//				SA_Singleton<GooglePlayManager>.Instance.UnlockAchievement("achievement5000Games");
//			}
//		}
//		if (GameCenterManager.IsInitialized && Application.platform == RuntimePlatform.IPhonePlayer)
//		{
//			if (this.roundsPlayed >= 100)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_roundsplayed100", true);
//			}
//			if (this.roundsPlayed >= 500)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_roundsplayed500", true);
//			}
//			if (this.roundsPlayed >= 1000)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_roundsplayed1000", true);
//			}
//			if (this.roundsPlayed >= 5000)
//			{
//				GameCenterManager.SubmitAchievement(100f, "bottleFlip_roundsplayed5000", true);
//			}
//		}
//	}
//
//	// Token: 0x060000BF RID: 191 RVA: 0x00007934 File Offset: 0x00005B34
//	public void SwapStackingBottles()
//	{
//		base.StartCoroutine("CheckAndSpawnStackingGem");
//		this.player.transform.DOKill(false);
//		this.playerC.RemoveStackingCharacter();
//		this.player.GetComponent<TrailRenderer>().enabled = false;
//		this.camC.MoveCameraUp();
//		this.player.transform.position = new Vector3(this.activeFloor.transform.position.x, this.player.transform.position.y + 2.66f, this.activeFloor.transform.position.z);
//		this.activeFloor.transform.DOMoveY(this.activeFloor.transform.position.y + 2.66f, 0.2f, false);
//		this.playerC.EquipStackingCharacter(UnityEngine.Object.Instantiate<GameObject>(this.stackingBottle));
//	}
//
//	// Token: 0x060000C0 RID: 192 RVA: 0x00007A34 File Offset: 0x00005C34
//	public void UnparentFloor()
//	{
//		if (GameController.gameMode == 2 && this.tempGameModeSelect == null)
//		{
//			this.bgFloor.transform.parent = null;
//		}
//	}
//
//	// Token: 0x0400001C RID: 28
//	public PlayerController playerC;
//
//	// Token: 0x0400001D RID: 29
//	public MyCameraController camC;
//
//	// Token: 0x0400001E RID: 30
//	public MeshRenderer floorMesh;
//
//	// Token: 0x0400001F RID: 31
//	public GameObject spawner;
//
//	// Token: 0x04000020 RID: 32
//	public GameObject floor;
//
//	// Token: 0x04000021 RID: 33
//	public GameObject[] floorArray;
//
//	// Token: 0x04000022 RID: 34
//	public GameObject fastFloor;
//
//	// Token: 0x04000023 RID: 35
//	public GameObject target;
//
//	// Token: 0x04000024 RID: 36
//	public GameObject activeFloor;
//
//	// Token: 0x04000025 RID: 37
//	public GameObject player;
//
//	// Token: 0x04000026 RID: 38
//	public GameObject chargeParticles;
//
//	// Token: 0x04000027 RID: 39
//	public GameObject[] stars;
//
//	// Token: 0x04000028 RID: 40
//	public TextMeshPro scoreText;
//
//	// Token: 0x04000029 RID: 41
//	public List<Transform> floorList;
//
//	// Token: 0x0400002A RID: 42
//	public GameObject plusOne;
//
//	// Token: 0x0400002B RID: 43
//	public GameObject plusX;
//
//	// Token: 0x0400002C RID: 44
//	public TextMeshPro perfectLabel;
//
//	// Token: 0x0400002D RID: 45
//	public TextMeshPro fastLabel;
//
//	// Token: 0x0400002E RID: 46
//	private Material activeFloorMat;
//
//	// Token: 0x0400002F RID: 47
//	public Material[] floorMats;
//
//	// Token: 0x04000030 RID: 48
//	private bool startButtonPressed;
//
//	// Token: 0x04000031 RID: 49
//	private bool gameOver = true;
//
//	// Token: 0x04000032 RID: 50
//	private bool gameStarted;
//
//	// Token: 0x04000033 RID: 51
//	public ObscuredInt score = 0;
//
//	// Token: 0x04000034 RID: 52
//	private int bestScore;
//
//	// Token: 0x04000035 RID: 53
//	private int floorsCounter;
//
//	// Token: 0x04000036 RID: 54
//	private float nextFloorScale;
//
//	// Token: 0x04000037 RID: 55
//	private float lastFloorScale = 1.5f;
//
//	// Token: 0x04000038 RID: 56
//	private int starnumber;
//
//	// Token: 0x04000039 RID: 57
//	private float duration;
//
//	// Token: 0x0400003A RID: 58
//	private float starscale;
//
//	// Token: 0x0400003B RID: 59
//	public bool isJumping;
//
//	// Token: 0x0400003C RID: 60
//	public bool fastFloorsActive;
//
//	// Token: 0x0400003D RID: 61
//	public float jumpPower;
//
//	// Token: 0x0400003E RID: 62
//	public Vector3 movementDirection;
//
//	// Token: 0x0400003F RID: 63
//	public SoundController soundC;
//
//	// Token: 0x04000040 RID: 64
//	public static int perfectHitCounter;
//
//	// Token: 0x04000041 RID: 65
//	public GameObject perfectHitSquare;
//
//	// Token: 0x04000042 RID: 66
//	private bool didCharge;
//
//	// Token: 0x04000043 RID: 67
//	private int random;
//
//	// Token: 0x04000044 RID: 68
//	public int activeFastFloor;
//
//	// Token: 0x04000045 RID: 69
//	private GameObject firstFloor;
//
//	// Token: 0x04000046 RID: 70
//	public Material skyBoxMat;
//
//	// Token: 0x04000047 RID: 71
//	public Color[] skyBoxColors1;
//
//	// Token: 0x04000048 RID: 72
//	public Color[] skyBoxColors2;
//
//	// Token: 0x04000049 RID: 73
//	public Color[] skyBoxColors3;
//
//	// Token: 0x0400004A RID: 74
//	public Color[] skyBoxColors4;
//
//	// Token: 0x0400004B RID: 75
//	public Color[] skyBoxColors5;
//
//	// Token: 0x0400004C RID: 76
//	public Color[] skyBoxColors6;
//
//	// Token: 0x0400004D RID: 77
//	public Color[] skyBoxColors7;
//
//	// Token: 0x0400004E RID: 78
//	public Color[] skyBoxColors8;
//
//	// Token: 0x0400004F RID: 79
//	public Color[] skyBoxColors9;
//
//	// Token: 0x04000050 RID: 80
//	public Color[] skyBoxColors10;
//
//	// Token: 0x04000051 RID: 81
//	private int activeFloorColor;
//
//	// Token: 0x04000052 RID: 82
//	private int lastFloorColor;
//
//	// Token: 0x04000053 RID: 83
//	private bool colorChangeActive;
//
//	// Token: 0x04000054 RID: 84
//	private Color tweeningColor;
//
//	// Token: 0x04000055 RID: 85
//	private int floorColorChangeChecker;
//
//	// Token: 0x04000056 RID: 86
//	public GameObject mainMenu;
//
//	// Token: 0x04000057 RID: 87
//	public GameObject titleText;
//
//	// Token: 0x04000058 RID: 88
//	public GameObject startGameCollider;
//
//	// Token: 0x04000059 RID: 89
//	public TextMeshPro tapToPlayText;
//
//	// Token: 0x0400005A RID: 90
//	private bool loopTapText;
//
//	// Token: 0x0400005B RID: 91
//	public GameObject[] mainMenuBtns;
//
//	// Token: 0x0400005C RID: 92
//	public TextMeshPro menuGemCount;
//
//	// Token: 0x0400005D RID: 93
//	public GameObject menuGemCountContainer;
//
//	// Token: 0x0400005E RID: 94
//	public SpriteRenderer[] mainMenuBtnSprites;
//
//	// Token: 0x0400005F RID: 95
//	public GameObject gameOverScreen;
//
//	// Token: 0x04000060 RID: 96
//	public GameObject gameOverLabel;
//
//	// Token: 0x04000061 RID: 97
//	public TextMeshPro gameOverHighScoreLabel;
//
//	// Token: 0x04000062 RID: 98
//	public TextMeshPro gameOverBestScoreLabel;
//
//	// Token: 0x04000063 RID: 99
//	public GameObject scoreElements;
//
//	// Token: 0x04000064 RID: 100
//	public GameObject[] gameOverScreenBtns;
//
//	// Token: 0x04000065 RID: 101
//	public SpriteRenderer[] gameOverBtnBGs;
//
//	// Token: 0x04000066 RID: 102
//	public Color btnBGColor;
//
//	// Token: 0x04000067 RID: 103
//	public GameObject avatar;
//
//	// Token: 0x04000068 RID: 104
//	public GameObject bonusLevelBar;
//
//	// Token: 0x04000069 RID: 105
//	public Transform bonusLevelBarAvatar;
//
//	// Token: 0x0400006A RID: 106
//	private int activeAvatar;
//
//	// Token: 0x0400006B RID: 107
//	private int bonusLevelCounter;
//
//	// Token: 0x0400006C RID: 108
//	public GameObject bonusLevelBtn;
//
//	// Token: 0x0400006D RID: 109
//	public GameObject bonusLevelSkipBtn;
//
//	// Token: 0x0400006E RID: 110
//	public GameObject bonusGameParticles;
//
//	// Token: 0x0400006F RID: 111
//	public GameObject gemPrefab;
//
//	// Token: 0x04000070 RID: 112
//	private ObscuredInt gemCount = 0;
//
//	// Token: 0x04000071 RID: 113
//	private Transform tempGem;
//
//	// Token: 0x04000072 RID: 114
//	private bool volumeOn = true;
//
//	// Token: 0x04000073 RID: 115
//	public GameObject settingsGO;
//
//	// Token: 0x04000074 RID: 116
//	public GameObject shopGO;
//
//	// Token: 0x04000075 RID: 117
//	private GameObject activeShopGO;
//
//	// Token: 0x04000076 RID: 118
//	private GameObject tempSettings;
//
//	// Token: 0x04000077 RID: 119
//	private int unlockedCharacters;
//
//	// Token: 0x04000078 RID: 120
//	private int cheapestPrice = 100;
//
//	// Token: 0x04000079 RID: 121
//	public GameObject shopExklamation;
//
//	// Token: 0x0400007A RID: 122
//	public GameObject shopSprite;
//
//	// Token: 0x0400007B RID: 123
//	public GameObject facebookBtn;
//
//	// Token: 0x0400007C RID: 124
//	public SpriteRenderer facebookBtnSprite;
//
//	// Token: 0x0400007D RID: 125
//	private bool fastJumpCounterActive;
//
//	// Token: 0x0400007E RID: 126
//	private float fastJumpCounter;
//
//	// Token: 0x0400007F RID: 127
//	private int activeCharacter;
//
//	// Token: 0x04000080 RID: 128
//	public GameObject[] characters;
//
//	// Token: 0x04000081 RID: 129
//	private ObscuredBool isPro = false;
//
//	// Token: 0x04000082 RID: 130
//	private AdScript adController;
//
//	// Token: 0x04000083 RID: 131
//	public GameObject videoAdButton;
//
//	// Token: 0x04000084 RID: 132
//	public GameObject shopVideoAdButton;
//
//	// Token: 0x04000085 RID: 133
//	private bool showingPopup;
//
//	// Token: 0x04000086 RID: 134
//	private Transform tempGemParent;
//
//	// Token: 0x04000087 RID: 135
//	public static bool bonusLevelActive;
//
//	// Token: 0x04000088 RID: 136
//	public GameObject bonusLevelFloor;
//
//	// Token: 0x04000089 RID: 137
//	private float bonusLevelCounterGoal = 50f;
//
//	// Token: 0x0400008A RID: 138
//	public Color bonusBackgroundColor;
//
//	// Token: 0x0400008B RID: 139
//	public int bonusGemCounter;
//
//	// Token: 0x0400008C RID: 140
//	public GameObject bonusBGParticles;
//
//	// Token: 0x0400008D RID: 141
//	public TextMeshPro bonusGameCountDown;
//
//	// Token: 0x0400008E RID: 142
//	public TextMeshPro bonusGemsCollected;
//
//	// Token: 0x0400008F RID: 143
//	public GameObject[] bonusGems;
//
//	// Token: 0x04000090 RID: 144
//	private int bonusGemValue = 1;
//
//	// Token: 0x04000091 RID: 145
//	public GameObject[] bonusPlusX;
//
//	// Token: 0x04000092 RID: 146
//	public GameObject[] bonusGemParticles;
//
//	// Token: 0x04000093 RID: 147
//	private int bonusGamesPlayed;
//
//	// Token: 0x04000094 RID: 148
//	private ObscuredInt roundsPlayed = 0;
//
//	// Token: 0x04000095 RID: 149
//	public TextMeshPro roundsPlayedText;
//
//	// Token: 0x04000096 RID: 150
//	public GameObject tutorialGO;
//
//	// Token: 0x04000097 RID: 151
//	public GameObject tutorialFinger;
//
//	// Token: 0x04000098 RID: 152
//	public GameObject tutorialFingerShadow;
//
//	// Token: 0x04000099 RID: 153
//	private bool tutorialActive;
//
//	// Token: 0x0400009A RID: 154
//	public TextMeshProFont chineseFont;
//
//	// Token: 0x0400009B RID: 155
//	private Texture2D texBG;
//
//	// Token: 0x0400009C RID: 156
//	private float savedMaxJumpPower;
//
//	// Token: 0x0400009D RID: 157
//	public GameObject targetpointer;
//
//	// Token: 0x0400009E RID: 158
//	public SpriteRenderer targetPointerSprite;
//
//	// Token: 0x0400009F RID: 159
//	public bool[] targetPointerColorArray;
//
//	// Token: 0x040000A0 RID: 160
//	private bool pressedBack;
//
//	// Token: 0x040000A1 RID: 161
//	public Color[] floorColors2;
//
//	// Token: 0x040000A2 RID: 162
//	public Material floor2BGMat;
//
//	// Token: 0x040000A3 RID: 163
//	public MeshRenderer floor2MR;
//
//	// Token: 0x040000A4 RID: 164
//	public GameObject newFloorGO;
//
//	// Token: 0x040000A5 RID: 165
//	public Vector3[] carpetPos;
//
//	// Token: 0x040000A6 RID: 166
//	public GameObject[] carpets;
//
//	// Token: 0x040000A7 RID: 167
//	public static int gameMode = 1;
//
//	// Token: 0x040000A8 RID: 168
//	private GameObject tempGameModeSelect;
//
//	// Token: 0x040000A9 RID: 169
//	public GameObject gameModeSelectGO;
//
//	// Token: 0x040000AA RID: 170
//	public GameObject speedFloor;
//
//	// Token: 0x040000AB RID: 171
//	public GameObject bowlingTable;
//
//	// Token: 0x040000AC RID: 172
//	public GameObject[] bowlingPins;
//
//	// Token: 0x040000AD RID: 173
//	private GameObject tempPins;
//
//	// Token: 0x040000AE RID: 174
//	public int bowlingCounter;
//
//	// Token: 0x040000AF RID: 175
//	private Vector3 bowlingSpawnerPos;
//
//	// Token: 0x040000B0 RID: 176
//	private int bowlingSavedRandom;
//
//	// Token: 0x040000B1 RID: 177
//	public int savedBowlingScore;
//
//	// Token: 0x040000B2 RID: 178
//	public TextMeshPro bowlingRoundCounterText;
//
//	// Token: 0x040000B3 RID: 179
//	public GameObject bgFloor;
//
//	// Token: 0x040000B4 RID: 180
//	public GameObject stackTable;
//
//	// Token: 0x040000B5 RID: 181
//	public GameObject stackTargetTable;
//
//	// Token: 0x040000B6 RID: 182
//	public GameObject stackBGWall;
//
//	// Token: 0x040000B7 RID: 183
//	public GameObject stackingBottle;
//
//	// Token: 0x040000B8 RID: 184
//	private GameObject stackingToBounce;
//}
