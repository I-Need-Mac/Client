using System.Collections;
using UnityEngine;

public enum AudioSource_Kind
{
    eEffect_00, eEffect_01, eEffect_02, eEffect_03, eEffect_04, eEffect_05, eEffect_06, eEffect_07, eEffect_08, eEffect_09
       , eEffect_10, eEffect_11, eEffect_12, eEffect_13, eEffect_14, eEffect_15, eEffect_16, eEffect_17, eEffect_18, eEffect_19
        , eVoice, eBGM, eNull
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] m_arrEffectAudioSource = new AudioSource[(int)AudioSource_Kind.eNull];

    private AudioSource m_comCurrentAudio;

    private bool m_isWait;
    private bool m_isInit = false;

    private float bgmVolume { 
        get{ return SettingManager.Instance.settings[Constants.BGM_VOLUME] / 10.0f;} 
    }
    private float effectVolume
    {
        get { return SettingManager.Instance.settings[Constants.EFFECT_VOLUME] / 10.0f; }
    }
    private float voiceVolume
    {
        get { return SettingManager.Instance.settings[Constants.VOICE_VOLUME] / 10.0f; }
    }
    public void Init()
    {
        if (!m_isInit)
        {
            m_isInit = true;
            GameObject objThis = this.gameObject;

            for (int i = 0; i < (int)AudioSource_Kind.eNull; i++)
            {
                m_arrEffectAudioSource[i] = objThis.AddComponent<AudioSource>();
                m_arrEffectAudioSource[i].loop = false;
                m_arrEffectAudioSource[i].playOnAwake = false;
            }

            m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].priority = 100;

            Audio_Source_Init();
        }
    }

    private void Audio_Source_Init()
    {
        //var data = Sc_GameManager.GetInstance.m_localDataManager.settingData;

        //for (int i = 0; i < (int)AudioSource_Kind.eVoice; i++)
        //{
        //    m_arrEffectAudioSource[i].volume = data.se;
        //}
        //m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].volume = data.voice;
        //m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = data.bgm;
    }


    #region BGM
    #region 클립 직접 넣기
    public void Play_BGM(AudioClip _clip)
    {
        StartCoroutine(Play_BGM_Coroutine(_clip));
    }

    private IEnumerator Play_BGM_Coroutine(AudioClip _clip)
    {
        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip != null)
        {
            if (!string.Equals(_clip.name, m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip.name))
            {
                if (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].isPlaying)
                {
                    yield return StartCoroutine(Stop_BGM_Smooth_Coroutine());
                }
                m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip = _clip;
                StartCoroutine(Play_BGM_Smooth_Coroutine());
            }
            else
            {
                StartCoroutine(Play_BGM_Smooth_Coroutine());
            }
        }
        else
        {
            m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip = _clip;
            m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].loop = true;
            Play_BGM_Smooth();
        }
    }
    #endregion

    public void Play_BGM_Smooth(float _mutiple = 2.5f)
    {
        StartCoroutine(Play_BGM_Smooth_Coroutine(_mutiple));
    }
    private IEnumerator Play_BGM_Smooth_Coroutine(float _mutiple = 2.5f)
    {
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = 0;
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].loop = true;
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].Play();

        while (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume < bgmVolume)
        {
            m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = Mathf.MoveTowards(m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume, bgmVolume, Time.deltaTime * _mutiple);
            yield return null;
        }
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = bgmVolume;
    }
    public void Stop_BGM_Smooth(float _mutiple = 2.5f)
    {
        StartCoroutine(Stop_BGM_Smooth_Coroutine(_mutiple));
    }
    public void Stop_BGM_RightNow()
    {
        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].isPlaying)
        {
            m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].Stop();
        }
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip = null;
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = bgmVolume;
    }
    private IEnumerator Stop_BGM_Smooth_Coroutine(float _mutiple = 2.5f)
    {
        if (_mutiple != 0)
        {
            if (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].isPlaying)
            {
                while (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume > 0)
                {
                    m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = Mathf.MoveTowards(m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume, 0, Time.deltaTime * _mutiple);
                    yield return null;
                }
                m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].Stop();
            }
        }

        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip = null;
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = bgmVolume;
    }
    public void Pause_BGM()
    {
        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip != null)
        {
            if (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].isPlaying)
            {
                m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].Pause();
            }
        }
    }
    public void Resume_BGM()
    {
        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].clip != null)
        {
            if (!m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].isPlaying)
            {
                m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].UnPause();
            }
        }
    }
    #endregion



    #region 효과음
    #region 클립 직접 넣기
    public void Play_EffectSound(AudioClip _clip, float _delay, bool _isLoop = false)
    {
        for (int i = (int)AudioSource_Kind.eEffect_00; i < (int)AudioSource_Kind.eVoice; i++)
        {
            if (!m_arrEffectAudioSource[i].isPlaying)
            {
                StartCoroutine(Play_EffectSound_Coroutine(_delay, i, _clip, _isLoop));
                break;
            }
        }
    }
    public void Play_EffectSound(AudioClip _clip, bool _isLoop = false, float loopTime = 0f)
    {
        for (int i = (int)AudioSource_Kind.eEffect_00; i < (int)AudioSource_Kind.eVoice; i++)
        {
            if (!m_arrEffectAudioSource[i].isPlaying)
            {
                StartCoroutine(Play_EffectSound_Coroutine(i, _clip, _isLoop, loopTime));
                break;
            }
        }
    }
    public float Play_EffectSound(AudioClip _clip)
    {
        for (int i = (int)AudioSource_Kind.eEffect_00; i < (int)AudioSource_Kind.eVoice; i++)
        {
            if (!m_arrEffectAudioSource[i].isPlaying)
            {
                StartCoroutine(Play_EffectSound_Coroutine(i, _clip, false, 0f));
                break;
            }
        }
        return _clip.length;
    }
    public void Play_EffectSound(int _index, AudioClip _clip, float _delay, bool _isLoop = false, float loopTime = 0f)
    {
        StartCoroutine(Play_EffectSound_Coroutine(_delay, _index, _clip, _isLoop));
    }
    public float Play_EffectSound(int _index, AudioClip _clip, bool _isLoop = false, float loopTime = 0f)
    {
        StartCoroutine(Play_EffectSound_Coroutine(_index, _clip, _isLoop, loopTime));

        return _clip.length;
    }
    private IEnumerator Play_EffectSound_Coroutine(int _index, AudioClip _clip, bool _isLoop, float loopTime)
    {
        if (m_arrEffectAudioSource[_index].clip != null)
        {
            if (m_arrEffectAudioSource[_index].isPlaying)
            {
                m_arrEffectAudioSource[_index].Stop();
            }
        }

        m_arrEffectAudioSource[_index].clip = _clip;
        m_arrEffectAudioSource[_index].loop = _isLoop;
        m_arrEffectAudioSource[_index].Play();

        yield return null;

        var currentLoopTime = loopTime;
        if (_isLoop && loopTime > 0)
        {
            var waitforUpdate = new WaitForUpdate();
            while (currentLoopTime >= 0f)
            {
                currentLoopTime -= Time.deltaTime;
                yield return waitforUpdate;
            }
            m_arrEffectAudioSource[_index].Stop();
        }

        yield return new WaitUntil(() => !m_arrEffectAudioSource[_index].isPlaying);
        m_arrEffectAudioSource[_index].clip = null;
    }
    private IEnumerator Play_EffectSound_Coroutine(float _delay, int _index, AudioClip _clip, bool _isLoop)
    {
        if (m_arrEffectAudioSource[_index].clip != null)
        {
            if (m_arrEffectAudioSource[_index].isPlaying)
            {
                m_arrEffectAudioSource[_index].Stop();
            }
        }
        yield return new WaitForSeconds(_delay);

        m_arrEffectAudioSource[_index].clip = _clip;
        m_arrEffectAudioSource[_index].loop = _isLoop;
        m_arrEffectAudioSource[_index].Play();

        yield return null;
        yield return new WaitUntil(() => !m_arrEffectAudioSource[_index].isPlaying);
        m_arrEffectAudioSource[_index].clip = null;
    }
    #endregion
    public void Stop_EffectSound(int _index, float _mutiple = 5, bool _isSmooth = true)
    {
        StartCoroutine(Stop_EffectSount_Coroutine(_index, _mutiple, _isSmooth));
    }

    private IEnumerator Stop_EffectSount_Coroutine(int _index, float _mutiple, bool _isSmooth)
    {
        if (_isSmooth)
        {
            if (m_arrEffectAudioSource[_index].isPlaying)
            {
                while (m_arrEffectAudioSource[_index].volume > 0)
                {
                    m_arrEffectAudioSource[_index].volume = Mathf.MoveTowards(m_arrEffectAudioSource[_index].volume, 0, Time.deltaTime * _mutiple);
                    yield return null;
                }
                m_arrEffectAudioSource[_index].Stop();
            }

            m_arrEffectAudioSource[_index].clip = null;
            m_arrEffectAudioSource[_index].volume = effectVolume;
        }
        else
        {
            if (m_arrEffectAudioSource[_index].clip != null)
            {
                m_arrEffectAudioSource[_index].Stop();
                m_arrEffectAudioSource[_index].clip = null;
            }
            m_arrEffectAudioSource[_index].loop = false;
        }
    }
    #endregion



    #region 보이스
    #region 클립 직접 넣기
    public float Play_VoiceSound(AudioClip _clip, int _priority)
    {
        StartCoroutine(Play_VoiceSound_Coroutine(_clip, _priority));
        return _clip.length;
    }
    public float Play_VoiceSound(AudioClip _clip, float _delay, int _priority = 128)
    {
        StartCoroutine(Play_VoiceSound_Coroutine(_clip, _delay, _priority));
        return _clip.length;
    }

    private IEnumerator Play_VoiceSound_Coroutine(AudioClip _clip, int _priority)
    {
        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip != null)
        {
            if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].isPlaying)
            {
                if (!string.Equals(m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip.name, _clip.name))
                {
                    if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority > _priority)
                    {
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Stop();
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = _clip;
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority = _priority;
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Play();
                    }
                }
            }
            else
            {
                m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = _clip;
                m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority = _priority;
                m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Play();
            }
        }
        else
        {
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = _clip;
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority = _priority;
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Play();
        }
        yield return null;
    }
    private IEnumerator Play_VoiceSound_Coroutine(AudioClip _clip, float _delay, int _priority = 128)
    {
        yield return new WaitForSeconds(_delay);

        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip != null)
        {
            if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].isPlaying)
            {
                if (!string.Equals(m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip.name, _clip.name))
                {
                    if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority > _priority)
                    {
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Stop();
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = _clip;
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority = _priority;
                        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Play();
                    }
                }
            }
            else
            {
                m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = _clip;
                m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority = _priority;
                m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Play();
            }
        }
        else
        {
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = _clip;
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].priority = _priority;
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Play();
        }
        yield return null;
    }

    public void Start_DialogueSound(AudioClip _clip)
    {
        StartCoroutine(DialogueSound(_clip));
    }
    private IEnumerator DialogueSound(AudioClip _clip)
    {
        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].isPlaying)
        {
            yield return StartCoroutine(DialogueSound_Stop());
        }

        StartCoroutine(DialogueSound_Play(_clip));
    }
    private IEnumerator DialogueSound_Play(AudioClip _clip)
    {
        yield return null;
        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].volume = voiceVolume;
        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = _clip;
        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Play();
        //yield return new WaitWhile(()=> m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].isPlaying);
        //m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Stop();
        //m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip = null;
    }
    #endregion

    public void DialogueSound_Stop_Smooth(float _muitple = 5)
    {
        StartCoroutine(DialogueSound_Stop(_muitple));
    }
    private IEnumerator DialogueSound_Stop(float _mutiple = 5)
    {
        while (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].volume > 0)
        {
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].volume = Mathf.MoveTowards(m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].volume, 0, Time.deltaTime * _mutiple);
            yield return null;
        }

        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Stop();
        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].volume = voiceVolume;
    }
    public float GetDialogue_Length()
    {
        return m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].clip.length;
    }
    public void DialogueSound_Stop_RightNow()
    {
        if (m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].isPlaying)
        {
            m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].Stop();
        }
    }
    #endregion


    public void AllEffectSound_Stop()
    {
        for (int i = 0; i < (int)AudioSource_Kind.eVoice; i++)
        {
            if (m_arrEffectAudioSource[i].clip != null)
            {
                if (m_arrEffectAudioSource[i].isPlaying)
                {
                    m_arrEffectAudioSource[i].Stop();
                }
                m_arrEffectAudioSource[i].clip = null;
            }
        }
    }

    public void AllEffectSound_Pause()
    {
        for (int i = 0; i < (int)AudioSource_Kind.eVoice; i++)
        {
            if (m_arrEffectAudioSource[i].clip != null)
            {
                if (m_arrEffectAudioSource[i].isPlaying)
                {
                    m_arrEffectAudioSource[i].Pause();
                }
            }
        }
    }
    public void AllEffectSound_Resume()
    {
        for (int i = 0; i < (int)AudioSource_Kind.eVoice; i++)
        {
            if (m_arrEffectAudioSource[i].clip != null)
            {
                if (m_arrEffectAudioSource[i].isPlaying)
                {
                    m_arrEffectAudioSource[i].UnPause();
                }
            }
        }
    }
    public void AllSound_Stop(bool _isSmooth = false, float _duration = 2.5f)
    {
        if (_isSmooth)
        {
            for (int i = 0; i < (int)AudioSource_Kind.eNull; i++)
            {
                if (m_arrEffectAudioSource[i].clip != null)
                {
                    if (m_arrEffectAudioSource[i].isPlaying)
                    {
                        m_arrEffectAudioSource[i].Stop();
                    }
                    m_arrEffectAudioSource[i].clip = null;
                }
            }
        }
        else
        {
            StartCoroutine(AllSound_Stop_Smooth(_duration));
        }
    }

    private IEnumerator AllSound_Stop_Smooth(float _duration)
    {
        for (int i = 0; i < (int)AudioSource_Kind.eNull; i++)
        {
            while (m_arrEffectAudioSource[i].volume > 0)
            {
                m_arrEffectAudioSource[i].volume = Mathf.MoveTowards(m_arrEffectAudioSource[i].volume, 0, Time.deltaTime * _duration);
                yield return null;
            }
            m_arrEffectAudioSource[i].Stop();
            m_arrEffectAudioSource[i].clip = null;
        }
        Refresh_BGM_Volum();
        Refresh_Effect_Volum();
        Refresh_Voice_Volum();
    }

    public void AllSound_Pause()
    {
        for (int i = 0; i < (int)AudioSource_Kind.eNull; i++)
        {
            /*if (m_arrEffectAudioSource[i].clip != null)
            {*/
            m_arrEffectAudioSource[i].Pause();
            //}
        }
    }
    public void AllSound_Resume()
    {
        for (int i = 0; i < (int)AudioSource_Kind.eNull; i++)
        {
            /*if (m_arrEffectAudioSource[i].clip != null)
            {*/
            m_arrEffectAudioSource[i].UnPause();
            //}
        }
    }


    public void Refresh_BGM_Volum()
    {
        m_arrEffectAudioSource[(int)AudioSource_Kind.eBGM].volume = bgmVolume;
    }
    public void Refresh_Effect_Volum()
    {
        for (int i = 0; i < (int)AudioSource_Kind.eVoice; i++)
        {
            m_arrEffectAudioSource[i].volume = m_arrEffectAudioSource[i].volume = effectVolume;
        }
    }
    public void Refresh_Voice_Volum()
    {
        m_arrEffectAudioSource[(int)AudioSource_Kind.eVoice].volume = voiceVolume;
    }

    public AudioSource GetAudioSource(int _index)
    {
        return m_arrEffectAudioSource[_index];
    }

    public bool GetisPlayEffectSound(AudioSource_Kind _kind)
    {
        return m_arrEffectAudioSource[(int)_kind].isPlaying;
    }

    public void SetVolum_Value(int _index, float _value)
    {
        m_arrEffectAudioSource[_index].volume = _value;
    }

    public void SetVolum_Ratio(int _index, float _ratio)
    {
        m_arrEffectAudioSource[_index].volume *= _ratio;
    }

    public void SetAllEffectVolum_Ratio(float ratio)
    {
        for (var i = 0; i < (int)AudioSource_Kind.eVoice; ++i)
        {
            m_arrEffectAudioSource[i].volume *= ratio;
        }
    }
    public void StopAllEffect()
    {
        for (var i = 0; i < (int)AudioSource_Kind.eVoice; ++i)
        {
            m_arrEffectAudioSource[i].Stop();
        }
    }

    public void SetPitch(int _index, float _value)
    {
        m_arrEffectAudioSource[_index].pitch = _value;
    }
}
