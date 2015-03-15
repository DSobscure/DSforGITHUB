using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LoginUI : MonoBehaviour {

    private string getAccount = "";
    private string getPassword = "";
    private string LoginResult = "";
    private string answerName = "";
    private string getAnswerKeyResult = "";

    IEnumerator Start()
    {

        PhotonGlobal.PS.LoginEvent += doLoginEvent;
        PhotonGlobal.PS.GetAnswerKeyEvent += doGetAnswerKeyEvent;

        yield return null;
    }
	
	// Update is called once per frame
	void OnGUI()
    {
        try
        {
            GUI.Label(new Rect(30, 10, 100, 20), "AK - ");

            if (PhotonGlobal.PS.ServerConnected)
            {
                GUI.Label(new Rect(130, 10, 100, 20), "Connecting . . .");

                if (AnswerGlobal.LoginStatus)
                {
                    try
                    {
                        if (AnswerGlobal.AnswerUniqueID != "")
                        {
                            if (GUI.Button(new Rect(30, 130, 100, 24), "進入遊戲"))
                            {
                                PhotonGlobal.PS.LoginEvent -= doLoginEvent;
                                PhotonGlobal.PS.GetAnswerKeyEvent -= doGetAnswerKeyEvent;
                                Application.LoadLevel("SoulCore");
                            }
                        }
                        else
                        {
                            GUI.Label(new Rect(30, 40, 300, 20), AnswerGlobal.AnswerUniqueID + "你現在還不需要知道這是什麼  就隨便填");

                            GUI.Label(new Rect(30, 70, 200, 20), "請填寫:");
                            answerName = GUI.TextField(new Rect(110, 100, 100, 20), answerName, 20);

                            if (answerName!="" && GUI.Button(new Rect(30, 130, 100, 24), "確認"))
                            {
                                PhotonGlobal.PS.GetAnswerKey(answerName, AnswerGlobal.Account, 1);
                            }

                            GUI.Label(new Rect(30, 160, 600, 20), getAnswerKeyResult);
                        }
                    }
                    catch (Exception EX)
                    {
                        Debug.Log(EX.Message);
                    }
                }
                else
                {
                    GUI.Label(new Rect(30, 40, 200, 20), "Please Login");

                    GUI.Label(new Rect(30, 70, 80, 20), "帳號:");
                    getAccount = GUI.TextField(new Rect(110, 70, 100, 20), getAccount, 17);

                    GUI.Label(new Rect(30, 100, 80, 20), "密碼:");
                    getPassword = GUI.PasswordField(new Rect(110, 100, 100, 20), getPassword, '*', 17);
                    if (GUI.Button(new Rect(30, 130, 100, 24), "登入"))
                    {
                        PhotonGlobal.PS.Login(getAccount, getPassword);
                    }

                    GUI.Label(new Rect(30, 160, 600, 20), LoginResult);
                }
            }
            else
            {
                GUI.Label(new Rect(130, 10, 200, 20), "Disconnect");
            }
        }
        catch (Exception EX)
        {
            Debug.Log(EX.Message);
        }
	}

    private void doLoginEvent(bool Status, string Message,string UniqueID,string answerUniqueID, string Account, string answerName,int soulLimit)
    {
        if (Status)
        {
            AnswerGlobal.UniqueID = UniqueID;
            AnswerGlobal.AnswerUniqueID = answerUniqueID;
            AnswerGlobal.Account = Account;
            AnswerGlobal.LoginStatus = true;
            AnswerGlobal.answer = new DSStructureForClient.Answer(answerUniqueID, answerName, Account, soulLimit);
        }
        else
        {
            AnswerGlobal.Account = "";
            AnswerGlobal.LoginStatus = false;
            LoginResult = Message;
        }
    }

    private void doGetAnswerKeyEvent(string uniqueID, string account)
    {
        AnswerGlobal.LoginStatus = true;
        AnswerGlobal.UniqueID = uniqueID;
        AnswerGlobal.Account = account;
    }
}
