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
    [SerializeField] private float backgroundDeflectionValue; //Arkaplan�n ka� birim hareket edece�i.
    [SerializeField] private float backgroundMovementDuration; //Arkaplan�n hareketinin ka� saniye s�rece�i.
    [SerializeField] private float dropEffectDeflectionValue; //A�a�� do�ru hareket miktar�.
    [SerializeField] private float buttonDropEffectDuration; //Butonun hareket s�resi.
    [SerializeField] private float dropEffectDuration; //Hareket s�resi.
    [SerializeField] private float sequanceDelay; //Sekanslar aras�na koyaca��m�z bekleme s�resi.

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
    [SerializeField] private Image background; //Screen arkaplan�.
    [SerializeField] private Image backgroundFrame; //Arkaplan �er�evesi.
    [SerializeField] private Image tick; //Tik i�areti.
    [SerializeField] private Image arrow; //Exp k�sm�ndaki ok i�areti.
    [SerializeField] private Image line1; //Exp k�sm�n�n alt�ndaki �izgi.
    [SerializeField] private Image coin; //Alt�n g�rseli.
    [SerializeField] private Image line2; //Alt�n k�sm�n�n alt�ndaki �izgi.
    [SerializeField] private Image scroll; //Scroll g�rseli.
    [SerializeField] private Image chest; //Sand�k g�rseli.

    [Space]
    [Space]
    [Header("Texts")]
    [Space]
    [SerializeField] private TextMeshProUGUI questCompletedText; //Quest completed yaz�s�.
    [SerializeField] private TextMeshProUGUI expTitleText;  //Exp ba�l���.
    [SerializeField] private TextMeshProUGUI expText; //Exp miktar�.
    [SerializeField] private TextMeshProUGUI coinTitleText;  //Alt�n ba�l���. 
    [SerializeField] private TextMeshProUGUI coinText; //Alt�n miktar�.
    [SerializeField] private TextMeshProUGUI scrollTitleText; //Scroll ba�l���.
    [SerializeField] private TextMeshProUGUI scrollText; //Scroll miktar�.
    [SerializeField] private TextMeshProUGUI chestText; //Chest yaz�s�.
    [SerializeField] private TextMeshProUGUI buttonText; //Butondaki yaz�.

    [Space]
    [Space]
    [Header("Rect Transforms")]
    [Space]
    [SerializeField] private Transform expArea; //Exp k�sm�.
    [SerializeField] private Transform coinArea; //Alt�n k�sm�. 
    [SerializeField] private Transform scrollArea; //Scroll k�sm�.
    [SerializeField] private Transform chestArea; //Chest k�sm�.


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

    private void OnScreenStart() //Screen a��ld���nda ger�ekle�ecek i�lemler.
    {
        var seq = DOTween.Sequence();


        seq.Join(background.transform.DOMoveY(background.transform.position.y - backgroundDeflectionValue, backgroundMovementDuration)).SetEase(backgroundMoveEase); //Arkaplan� a��l��ta hareket ettir.
        seq.Join(background.DOColor(halfTransparentBlackColor, backgroundMovementDuration)); //Arkaplan�n rengini belirginle�tir.
        seq.Join(backgroundFrame.DOColor(halfTransparentWhiteColor, backgroundMovementDuration)); //Arkaplan �er�evesinin rengini belirginle�tir.

        seq.Append(questCompletedText.rectTransform.DOMoveY(questCompletedText.transform.position.y - dropEffectDeflectionValue, 0)); //Quest tamamland� texti ile ilgili i�lemler.
        seq.Append(questCompletedText.DOColor(yellowColor, dropEffectDuration));
        seq.Join(questCompletedText.transform.DOScale(Vector3.one, dropEffectDuration).SetEase(dropEffectEase));

        seq.Append(tick.rectTransform.DOMoveY(tick.transform.position.y - dropEffectDeflectionValue, 0)); //Tick i�areti ile ilgili i�lemler.
        seq.Append(tick.DOColor(yellowColor, dropEffectDuration));
        seq.Join(tick.transform.DOScale(Vector3.one, dropEffectDuration).SetEase(dropEffectEase));


        seq.Join(expArea.DOMoveY(expArea.position.y + dropEffectDeflectionValue, 0));


        seq.Append(expArea.DOMoveY(expArea.position.y - dropEffectDeflectionValue, dropEffectDuration)); //Exp b�lgesi ile ilgili i�lemler.
        seq.Join(arrow.DOColor(whiteColor, dropEffectDuration));
        seq.Join(line1.DOColor(quarterTransparentWhiteColor, dropEffectDuration));
        seq.Join(expTitleText.DOColor(whiteColor, dropEffectDuration));
        seq.Join(expText.DOColor(whiteColor, dropEffectDuration));


        seq.Join(coinArea.DOMoveY(coinArea.position.y + dropEffectDeflectionValue, 0));

        seq.AppendInterval(dropEffectDuration);

        seq.Append(coinArea.DOMoveY(coinArea.position.y - dropEffectDeflectionValue, dropEffectDuration)); //Coin b�lgesi ile ilgili i�lemler.
        seq.Join(coin.DOColor(whiteColor, dropEffectDuration));
        seq.Join(line2.DOColor(quarterTransparentWhiteColor, dropEffectDuration));
        seq.Join(coinTitleText.DOColor(whiteColor, dropEffectDuration));
        seq.Join(coinText.DOColor(new Color(coinText.color.r, coinText.color.g, coinText.color.b, 1), dropEffectDuration));


        seq.Join(scrollArea.DOMoveY(scrollArea.position.y + dropEffectDeflectionValue, 0));

        seq.AppendInterval(dropEffectDuration);

        seq.Append(scrollArea.DOMoveY(scrollArea.position.y - dropEffectDeflectionValue, dropEffectDuration)); //Scroll b�lgesi ile ilgili i�lemler.
        seq.Join(scroll.DOColor(whiteColor, dropEffectDuration));
        seq.Join(scrollTitleText.DOColor(whiteColor, dropEffectDuration));
        seq.Join(scrollText.DOColor(new Color(scrollText.color.r, coinText.color.g, coinText.color.b, 1), dropEffectDuration));

        seq.AppendInterval(dropEffectDuration);

        seq.Append(chest.transform.DOScale(Vector3.one, dropEffectDuration*2)).SetEase(dropEffectEase); //Chest ikonunu normal boyutuna getir. 
        seq.Join(chestText.transform.DOScale(Vector3.one, dropEffectDuration*2)).SetEase(dropEffectEase); //Chest textini normal boyutuna getir.
        seq.Join(chest.DOColor(whiteColor, dropEffectDuration*2));
        seq.Join(chestText.DOColor(new Color(chestText.color.r, chestText.color.g, chestText.color.b, 1), dropEffectDuration*2));


        seq.Join(claimButton.transform.DOMoveY(claimButton.transform.position.y + dropEffectDeflectionValue, 0)); //Butonu bir miktar yukar� hareket ettir.

        seq.AppendInterval(dropEffectDuration*1.5f);

        seq.Append(claimButton.image.DOColor(whiteColor, dropEffectDuration)); //Butonun rengini g�r�n�r yap.
        seq.Join(claimButton.transform.DOMoveY(claimButton.transform.position.y - dropEffectDeflectionValue, buttonDropEffectDuration)); //Butonun konumuna getir.
        seq.Join(buttonText.DOColor(whiteColor, dropEffectDuration)); //Buton textinin rengini g�r�n�r yap.

    }

    private void SetStartAllElements() //T�m g�rsel ve yaz�lar�n ba�lang�� ayar�n� yapt���m�z fonksiyon.
    {
        background.transform.position += Vector3.up * backgroundDeflectionValue; //Arkaplan� bir miktar yukar� hareket ettir.
        questCompletedText.transform.localScale = Vector3.one * 1.5f; //Quest tamamland� yaz�s�n� b�y�t.
        tick.transform.localScale = Vector3.one * 1.5f; //Tick i�aretini b�y�t.  
        chest.transform.DOScale(Vector3.one * 1.5f, 0); //Chest ikonunu b�y�t.
        chestText.transform.DOScale(Vector3.one * 1.5f, 0); //Chest textini b�y�t.

        foreach (var item in allImages) //Ba�lang��ta t�m g�rsellerin rengini �effaf yap.
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0f);

        }

        foreach (var item in allTexts) //Ba�lang��ta t�m textlerin rengini �effaf yap.
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
