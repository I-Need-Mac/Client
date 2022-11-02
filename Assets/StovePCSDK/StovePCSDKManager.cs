using Stove.PCSDK.NET;
using System;
using System.Collections;
using System.Text;
using UnityEngine;

[DisallowMultipleComponent]
public class StovePCSDKManager : MonoBehaviour
{
    private static StovePCSDKManager instance;
    public static StovePCSDKManager Instance
    {
        get
        {
            if (instance == null)
            {
                return new GameObject("StovePCSDKManager").AddComponent<StovePCSDKManager>();
            }
            else
            {
                return instance;
            }
        }
    }

    [SerializeField]
    private string env;
    [SerializeField]
    private string appKey;
    [SerializeField]
    private string appSecret;
    [SerializeField]
    private string gameId;
    [SerializeField]
    private StovePCLogLevel logLevel;
    [SerializeField]
    private string logPath;
    [SerializeField]
    private float RunCallbacksIntervalInSeconds = 1f;

    private Coroutine runcallbacksCoroutine;

    public StovePCUser User { get; private set; }
    public StovePCOwnership[] Ownerships { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        ProceedPCSDKSteps();
    }

    private void OnDestroy()
    {
        if (runcallbacksCoroutine != null)
        {
            StopCoroutine(runcallbacksCoroutine);
            runcallbacksCoroutine = null;
        }

        StovePC.Uninitialize();
    }

    private IEnumerator RunCallbacks(float intervalSeconds)
    {
        WaitForSeconds wfs = new WaitForSeconds(intervalSeconds);
        while (true)
        {
            StovePC.RunCallback();
            yield return wfs;
        }
    }

    private void WriteLog(string log)
    {
        // 어쩌면 당신은 스크립트의 로그를 게임에서 사용하는 로깅 시스템에 출력하고 싶어 할지도 모릅니다.
        // 그렇다면 여기에 당신만의 로깅을 구현하십시오.
        Debug.Log(string.Concat(log, Environment.NewLine));
    }

    private void ProceedPCSDKSteps()
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("ProceedPCSDKSteps");
        sb.AppendFormat(" - nothing");
        WriteLog(sb.ToString());
        #endregion

        StovePCConfig config = new StovePCConfig
        {
            Env = this.env,
            AppKey = this.appKey,
            AppSecret = this.appSecret,
            GameId = this.gameId,
            LogLevel = this.logLevel,
            LogPath = this.logPath
        };

        StovePCCallback callback = new StovePCCallback
        {
            OnError = new StovePCErrorDelegate(this.OnError),
            OnInitializationComplete = new StovePCInitializationCompleteDelegate(this.OnInitializationComplete),
            OnUser = new StovePCUserDelegate(this.OnUser),
            OnOwnership = new StovePCOwnershipDelegate(this.OnOwnership)
        };

        StovePCResult callResult = StovePC.Initialize(config, callback);
        HandleStoveCallResult(StovePCFunctionType.Initialize, callResult);
    }

    private void OnError(StovePCError error)
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnError");
        sb.AppendFormat(" - error.FunctionType : {0}" + Environment.NewLine, error.FunctionType.ToString());
        sb.AppendFormat(" - error.Result : {0}" + Environment.NewLine, (int)error.Result);
        sb.AppendFormat(" - error.Message : {0}" + Environment.NewLine, error.Message);
        sb.AppendFormat(" - error.ExternalError : {0}", error.ExternalError.ToString());
        WriteLog(sb.ToString());
        #endregion

        switch (error.FunctionType)
        {
            case StovePCFunctionType.Initialize:
            case StovePCFunctionType.GetUser:
            case StovePCFunctionType.GetOwnership:
                BeginQuitAppDueToError();
                break;
        }
    }

    private void OnInitializationComplete()
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnInitializationComplete");
        sb.AppendFormat(" - nothing");
        WriteLog(sb.ToString());
        #endregion

        if (this.User.MemberNo == 0)
        {
            StovePCResult callResult = StovePC.GetUser();
            HandleStoveCallResult(StovePCFunctionType.GetUser, callResult);
        }
    }

    private void OnUser(StovePCUser user)
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnUser");
        sb.AppendFormat(" - user.MemberNo : {0}" + Environment.NewLine, user.MemberNo.ToString());
        sb.AppendFormat(" - user.Nickname : {0}" + Environment.NewLine, user.Nickname);
        WriteLog(sb.ToString());
        #endregion

        this.User = user;

        if (Ownerships == null)
        {
            StovePCResult callResult = StovePC.GetOwnership();
            HandleStoveCallResult(StovePCFunctionType.GetOwnership, callResult);
        }
    }

    private void OnOwnership(StovePCOwnership[] ownerships)
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnOwnership");
        sb.AppendFormat(" - ownerships.Length : {0}" + Environment.NewLine, ownerships.Length);
        for (int i = 0; i < ownerships.Length; i++)
        {
            sb.AppendFormat(" - ownerships[{0}].GameId : {1}" + Environment.NewLine, i, ownerships[i].GameId);
            sb.AppendFormat(" - ownerships[{0}].GameCode : {1}" + Environment.NewLine, i, ownerships[i].GameCode);
            sb.AppendFormat(" - ownerships[{0}].OwnershipCode : {1}" + Environment.NewLine, i, ownerships[i].OwnershipCode);
            sb.AppendFormat(" - ownerships[{0}].PurchaseDate : {1}" + Environment.NewLine, i, ownerships[i].PurchaseDate);
            sb.AppendFormat(" - ownerships[{0}].MemberNo : {1}", i, ownerships[i].MemberNo.ToString());

            if (i < ownerships.Length - 1)
                sb.AppendFormat(Environment.NewLine);
        }
        WriteLog(sb.ToString());
        #endregion

        this.Ownerships = ownerships;

        bool owned = false;

        foreach (var ownership in ownerships)
        {
            // [OwnershipCode] 1: 소유권 획득, 2: 소유권 해제(구매 취소한 경우)
            if (ownership.MemberNo != this.User.MemberNo || ownership.OwnershipCode != 1)
            {
                continue;
            }

            // [GameCode] 3: 기본게임, 5: DLC
            if (ownership.GameId == this.gameId && ownership.GameCode == 3)
            {
                // 기본게임 구매 사용자에 대한 처리
                owned = true;
            }

            // DLC를 판매하는 게임일 때만 필요
            if (ownership.GameId == "YOUR_DLC_ID" && ownership.GameCode == 5)
            {
                // YOUR_DLC_ID(DLC) 구매 사용자에 대한 처리
            }
        }

        if (owned)
        {
            EnterGameWorld();
        }
        else
        {
            BeginQuitAppDueToOwnership();
        }
    }

    private void HandleStoveCallResult(StovePCFunctionType type, StovePCResult result)
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("HandleStoveCallResult");
        sb.AppendFormat(" - type : {0}" + Environment.NewLine, type.ToString());
        sb.AppendFormat(" - result : {0}", result.ToString());
        WriteLog(sb.ToString());
        #endregion

        if (result == StovePCResult.NoError)
        {
            if (type == StovePCFunctionType.Initialize)
            {
                runcallbacksCoroutine = StartCoroutine(RunCallbacks(RunCallbacksIntervalInSeconds));
            }
        }
        else
        {
            switch (type)
            {
                case StovePCFunctionType.Initialize:
                case StovePCFunctionType.GetUser:
                case StovePCFunctionType.GetOwnership:
                    BeginQuitAppDueToError();
                    break;
            }
        }
    }

    private void BeginQuitAppDueToError()
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("BeginQuitAppDueToError");
        sb.AppendFormat(" - nothing");
        WriteLog(sb.ToString());
        #endregion

        // 어쩌면 당신은 즉시 앱을 중단하기보다는 사용자에게 앱 중단에 대한 메시지를 보여준 후
        // 사용자 액션(e.g. 종료 버튼 클릭)에 따라 앱을 중단하고 싶어 할지도 모릅니다.
        // 그렇다면 여기에 QuitApplication을 지우고 당신만의 로직을 구현하십시오.
        // 권장하는 필수 사전 작업 오류에 대한 메시지는 아래와 같습니다.
        // 한국어 : 필수 사전 작업이 실패하여 게임을 종료합니다.
        // 그 외 언어 : The required pre-task fails and exits the game.
        QuitApplication();
    }

    private void BeginQuitAppDueToOwnership()
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("BeginQuitAppDueToOwnership");
        sb.AppendFormat(" - nothing");
        WriteLog(sb.ToString());
        #endregion

        // 어쩌면 당신은 즉시 앱을 중단하기보다는 사용자에게 앱 중단에 대한 메시지를 보여준 후
        // 사용자 액션(e.g. 종료 버튼 클릭)에 따라 앱을 중단하고 싶어 할지도 모릅니다.
        // 그렇다면 여기에 QuitApplication을 지우고 당신만의 로직을 구현하십시오.
        // 권장하는 소유권 없음에 대한 메시지는 아래와 같습니다.
        // 한국어 : 게임을 구매한 계정으로 STOVE 클라이언트에 로그인하시기 바랍니다.
        // 그 외 언어: Please log in to STOVE Client with the account that has purchased the game.
        QuitApplication();
    }

    private void EnterGameWorld()
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("EnterGameWorld");
        sb.AppendFormat(" - nothing");
        WriteLog(sb.ToString());
        #endregion

        // 게임월드로 진입하는 당신만의 로직을 여기에 구현하십시오.
        // 예를 들어 Application.LoadLevel 또는 SceneManager.LoadScene 메서드를 통해 씬 전환을 할 수도 있고
        // 현재 씬의 게임오브젝트를 활성화 할 수도 있습니다.
    }

    public void QuitApplication()
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("QuitApplication");
        sb.AppendFormat(" - nothing");
        WriteLog(sb.ToString());
        #endregion

        Application.Quit();
    }
}