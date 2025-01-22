using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Shared _shared;
    [SerializeField] private TMP_Text _txtLevelName;
    [SerializeField] private TMP_Text _txtPoints;
    [SerializeField] private TMP_Text _txtLife;
    [SerializeField] private CanvasGroup _cgBoss;
    [SerializeField] private TMP_Text _txtBossName;
    [SerializeField] private Image _imgBossGauge;


    private void Start()
    {
        _shared.OnBeginBossBattle.AddListener(OnBeginBossBattle);
        _txtLevelName.text = _shared.CurrentLevel.LevelName;
        _txtPoints.text = _shared.TotalPoints.ToString();
        _cgBoss.alpha = 0f;
    }

    private void OnBeginBossBattle()
    {
        _cgBoss.alpha = 1f;
        _txtBossName.text = _shared.Boss.DisplayName;
    }

    private void Update()
    {
        _txtPoints.text = _shared.TotalPoints.ToString();
        _txtLife.text = _shared.Player.Life.ToString();
        if(_shared.Boss)
        {
            _imgBossGauge.fillAmount = _shared.Boss.LifePercent;
        }
    }
}
