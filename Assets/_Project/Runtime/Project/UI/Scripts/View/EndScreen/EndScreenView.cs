using _Project.Runtime.Core.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using _Project.Runtime.Project.UI.Scripts.View;

public class EndScreenView : SingletonBehaviour<EndScreenView>
{
    [Header("Transition Variables")]
    [Space]
    [SerializeField] private float backgroundDeflectionValue; //Arkaplanýn kaç birim hareket edeceði.
    [SerializeField] private float backgroundMovementDuration; //Arkaplanýn hareketinin kaç saniye süreceði.
    [SerializeField] private float dropEffectDeflectionValue; //Aþaðý doðru hareket miktarý.
    [SerializeField] private float buttonDropEffectDuration; //Butonun hareket süresi.
    [SerializeField] private float dropEffectDuration; //Hareket süresi.
    [SerializeField] private float sequanceDelay; //Sekanslar arasýna koyacaðýmýz bekleme süresi.

    [Space]
    [Space]
    [Header("Colors")]
    [Space]
    [SerializeField] private Color whiteColor;
    [SerializeField] private Color transparentWhiteColor;
    [SerializeField] private Color yellowColor;
    [SerializeField] private Color transparentYellowColor;
    [SerializeField] private Color blackColor;
    [SerializeField] private Color transparentBlackColor;
    [SerializeField] private Color halfTransparentWhiteColor;
    [SerializeField] private Color halfTransparentBlackColor;
    [SerializeField] private Color quarterTransparentWhiteColor;

    [Space]
    [Space]
    [Header("Images")]
    [Space]
    [SerializeField] private Image background; //Screen arkaplaný.
    [SerializeField] private Image backgroundFrame; //Arkaplan çerçevesi.
    [SerializeField] private Image tick; //Tik iþareti.
    [SerializeField] private Image arrow; //Exp kýsmýndaki ok iþareti.
    [SerializeField] private Image line1; //Exp kýsmýnýn altýndaki çizgi.
    [SerializeField] private Image coin; //Altýn görseli.
    [SerializeField] private Image line2; //Altýn kýsmýnýn altýndaki çizgi.
    [SerializeField] private Image scroll; //Scroll görseli.
    [SerializeField] private Image chest; //Sandýk görseli.

    [Space]
    [Space]
    [Header("Texts")]
    [Space]
    [SerializeField] private TextMeshProUGUI questCompletedText; //Quest completed yazýsý.
    [SerializeField] private TextMeshProUGUI expTitleText;  //Exp baþlýðý.
    [SerializeField] private TextMeshProUGUI expText; //Exp miktarý.
    [SerializeField] private TextMeshProUGUI coinTitleText;  //Altýn baþlýðý. 
    [SerializeField] private TextMeshProUGUI coinText; //Altýn miktarý.
    [SerializeField] private TextMeshProUGUI scrollTitleText; //Scroll baþlýðý.
    [SerializeField] private TextMeshProUGUI scrollText; //Scroll miktarý.
    [SerializeField] private TextMeshProUGUI chestText; //Chest yazýsý.
    [SerializeField] private TextMeshProUGUI buttonText; //Butondaki yazý.

    [Space]
    [Space]
    [Header("Rect Transforms")]
    [Space]
    [SerializeField] private Transform expArea; //Exp kýsmý.
    [SerializeField] private Transform coinArea; //Altýn kýsmý. 
    [SerializeField] private Transform scrollArea; //Scroll kýsmý.
    [SerializeField] private Transform chestArea; //Chest kýsmý.


    [Space]
    [Space]
    [Header("Other Variables")]
    [Space]
    [SerializeField] private Ease backgroundMoveEase;
    [SerializeField] private Ease dropEffectEase;
    [SerializeField] private Button claimButton;
    [SerializeField] private Image[] allImages;
    [SerializeField] private TextMeshProUGUI[] allTexts;
    [SerializeField] private RectTransform[] allAreas;

    private void Awake()
    {



    }

    private void Start()
    {
        SetStartAllElements();


    }

    private void OnScreenStart() //Screen açýldýðýnda gerçekleþecek iþlemler.
    {
        var seq = DOTween.Sequence();


        seq.Join(background.transform.DOMoveY(background.transform.position.y - backgroundDeflectionValue, backgroundMovementDuration)).SetEase(backgroundMoveEase); //Arkaplaný açýlýþta hareket ettir.
        seq.Join(background.DOColor(halfTransparentBlackColor, backgroundMovementDuration)); //Arkaplanýn rengini belirginleþtir.
        seq.Join(backgroundFrame.DOColor(halfTransparentWhiteColor, backgroundMovementDuration)); //Arkaplan çerçevesinin rengini belirginleþtir.

        seq.Append(questCompletedText.rectTransform.DOMoveY(questCompletedText.transform.position.y - dropEffectDeflectionValue, 0)); //Quest tamamlandý texti ile ilgili iþlemler.
        seq.Append(questCompletedText.DOColor(yellowColor, dropEffectDuration));
        seq.Join(questCompletedText.transform.DOScale(Vector3.one, dropEffectDuration).SetEase(dropEffectEase));

        seq.Append(tick.rectTransform.DOMoveY(tick.transform.position.y - dropEffectDeflectionValue, 0)); //Tick iþareti ile ilgili iþlemler.
        seq.Append(tick.DOColor(yellowColor, dropEffectDuration));
        seq.Join(tick.transform.DOScale(Vector3.one, dropEffectDuration).SetEase(dropEffectEase));


        seq.Join(expArea.DOMoveY(expArea.position.y + dropEffectDeflectionValue, 0));


        seq.Append(expArea.DOMoveY(expArea.position.y - dropEffectDeflectionValue, dropEffectDuration)); //Exp bölgesi ile ilgili iþlemler.
        seq.Join(arrow.DOColor(whiteColor, dropEffectDuration));
        seq.Join(line1.DOColor(quarterTransparentWhiteColor, dropEffectDuration));
        seq.Join(expTitleText.DOColor(whiteColor, dropEffectDuration));
        seq.Join(expText.DOColor(whiteColor, dropEffectDuration));


        seq.Join(coinArea.DOMoveY(coinArea.position.y + dropEffectDeflectionValue, 0));

        seq.AppendInterval(dropEffectDuration);

        seq.Append(coinArea.DOMoveY(coinArea.position.y - dropEffectDeflectionValue, dropEffectDuration)); //Coin bölgesi ile ilgili iþlemler.
        seq.Join(coin.DOColor(whiteColor, dropEffectDuration));
        seq.Join(line2.DOColor(quarterTransparentWhiteColor, dropEffectDuration));
        seq.Join(coinTitleText.DOColor(whiteColor, dropEffectDuration));
        seq.Join(coinText.DOColor(new Color(coinText.color.r, coinText.color.g, coinText.color.b, 1), dropEffectDuration));


        seq.Join(scrollArea.DOMoveY(scrollArea.position.y + dropEffectDeflectionValue, 0));

        seq.AppendInterval(dropEffectDuration);

        seq.Append(scrollArea.DOMoveY(scrollArea.position.y - dropEffectDeflectionValue, dropEffectDuration)); //Scroll bölgesi ile ilgili iþlemler.
        seq.Join(scroll.DOColor(whiteColor, dropEffectDuration));
        seq.Join(scrollTitleText.DOColor(whiteColor, dropEffectDuration));
        seq.Join(scrollText.DOColor(new Color(scrollText.color.r, coinText.color.g, coinText.color.b, 1), dropEffectDuration));

        seq.AppendInterval(dropEffectDuration);

        seq.Append(chest.transform.DOScale(Vector3.one, dropEffectDuration*2)).SetEase(dropEffectEase); //Chest ikonunu normal boyutuna getir. 
        seq.Join(chestText.transform.DOScale(Vector3.one, dropEffectDuration*2)).SetEase(dropEffectEase); //Chest textini normal boyutuna getir.
        seq.Join(chest.DOColor(whiteColor, dropEffectDuration*2));
        seq.Join(chestText.DOColor(new Color(chestText.color.r, chestText.color.g, chestText.color.b, 1), dropEffectDuration*2));


        seq.Join(claimButton.transform.DOMoveY(claimButton.transform.position.y + dropEffectDeflectionValue, 0)); //Butonu bir miktar yukarý hareket ettir.

        seq.AppendInterval(dropEffectDuration*1.5f);

        seq.Append(claimButton.image.DOColor(whiteColor, dropEffectDuration)); //Butonun rengini görünür yap.
        seq.Join(claimButton.transform.DOMoveY(claimButton.transform.position.y - dropEffectDeflectionValue, buttonDropEffectDuration)); //Butonun konumuna getir.
        seq.Join(buttonText.DOColor(whiteColor, dropEffectDuration)); //Buton textinin rengini görünür yap.

    }

    private void SetStartAllElements() //Tüm görsel ve yazýlarýn baþlangýç ayarýný yaptýðýmýz fonksiyon.
    {
        background.transform.position += Vector3.up * backgroundDeflectionValue; //Arkaplaný bir miktar yukarý hareket ettir.
        questCompletedText.transform.localScale = Vector3.one * 1.5f; //Quest tamamlandý yazýsýný büyüt.
        tick.transform.localScale = Vector3.one * 1.5f; //Tick iþaretini büyüt.  
        chest.transform.DOScale(Vector3.one * 1.5f, 0); //Chest ikonunu büyüt.
        chestText.transform.DOScale(Vector3.one * 1.5f, 0); //Chest textini büyüt.

        foreach (var item in allImages) //Baþlangýçta tüm görsellerin rengini þeffaf yap.
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);

        }

        foreach (var item in allTexts) //Baþlangýçta tüm textlerin rengini þeffaf yap.
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);
        }



        background.color = transparentBlackColor;
        backgroundFrame.DOColor(transparentWhiteColor, 0f).OnComplete(() =>
        {
            OnScreenStart();
        });

    }

    public void RestartScene()
    {
        InventoryScreenView.isPotionCrafted = false;
        SceneManager.LoadScene(0);
    }

    

}
