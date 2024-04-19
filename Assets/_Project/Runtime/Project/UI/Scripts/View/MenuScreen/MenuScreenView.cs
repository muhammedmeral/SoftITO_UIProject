using _Project.Runtime.Core.Singleton;
using _Project.Runtime.Core.UI.Scripts.Manager;
using _Project.Runtime.Project.Launcher.Scripts.Manager.Bootstrap;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;
using System.Threading.Tasks;

namespace _Project.Runtime.Project.UI.Scripts.View
{
    public class MenuScreenView : SingletonBehaviour<MenuScreenView>
    {

        [Header("Transition Variables")]
        [Space]
        [SerializeField] private float mainAreasDeflectionValue; //Ana k�s�mlar�n ka� birim hareket edece�i.
        [SerializeField] private float mainAreasMovementDuration; //Ana birimlerin hareket s�resi.
        [SerializeField] private float questCompleteDuration; //Quest tamamland���nda renk de�i�iminin s�resi.
        [SerializeField] private string[] questTitleDescriptions; //Quest ba�l���nda yazacak yaz�lar.
        [SerializeField] private string quest3CompletedExplanation; //Quest3 tamamland���nda yazacak yaz�.

        [Space]
        [Space]
        [Header("Colors")]
        [Space]
        [SerializeField] private Color whiteColor;
        [SerializeField] private Color yellowColor;
        [SerializeField] private Color halfTransparentWhiteColor;
        [SerializeField] private Color transparentBlackColor;
        [SerializeField] private Color buttonClickedWhiteColor;

        [Space]
        [Space]
        [Header("Images")]
        [Space]
        [SerializeField] private Image[] quest2CompletedBackground; //Quest2 tamamland���nda aktif olacak ye�il arkaplan.
        [SerializeField] private Image[] quest2Background; //Quest2 tamamlanmam��sa aktif olacak arkaplan.
        [SerializeField] private Image[] quest3CompletedBackground; //Quest3 tamamland���nda aktif olacak ye�il arkaplan.
        [SerializeField] private Image[] quest3Background; //Quest3 tamamlanmam��sa aktif olacak arkaplan.
        [SerializeField] private Image bluredBackground; //Sahne ge�i�i s�ras�nda kullanaca��m�z bulan�k arkaplan.
        [SerializeField] private Image potionButtonImage; //�ksir butonunun image componenti.
        [SerializeField] private Image potionEffectBackground; //�ksir butonunun etki yaz�s�n�n arkaplan�.
        [SerializeField] private Image potionInfo; //�ksir butonun yan�nda g�r�necek info butonu.
        [SerializeField] private Image inventoryButtonInfo; //Envanter butonun yan�nda g�r�nen info g�rseli.        
        [SerializeField] private Image inventoryButtonFrame; //Envanter butonu g�rseli.        
        [SerializeField] private Image quest2Tick; //Qutes2 tamamland���nda aktif olacak tik i�areti.
        [SerializeField] private Image quest2Star; //Quest2 tamamlanmad���nda aktif olacak y�ld�z i�areti.
        [SerializeField] private Image quest3Tick; //Quest3 tamamland���nda aktif olacak tik i�areti.
        [SerializeField] private Image quest3Star; //Quest3 tamamlanmad���nda aktif olacak y�ld�z i�areti.
        [SerializeField] private Image consumableSlot;

        [Space]
        [Space]
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI potionEffectText; //�ksir butonun etki yaz�s�.
        [SerializeField] private TextMeshProUGUI questsTitleText; //Quest ba�l���.
        [SerializeField] private TextMeshProUGUI quest3Text; //Quest3'�n a��klama yaz�s�.

        [Space]
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button inventoryButton; //Envanter butonu.
        [SerializeField] private Button potionButton; //�ksir butonu

        [Space]
        [Space]
        [Header("Main Areas")]
        [Space]
        [SerializeField] private RectTransform topArea; //�st ana b�lge.
        [SerializeField] private RectTransform bottomArea; //Alt ana b�lge.
        [SerializeField] private RectTransform rightArea; //Sa� ana b�lge.
        [SerializeField] private RectTransform leftArea; //Sol ana b�lge.

        private void Start()
        {
            if (InventoryScreenView.isPotionCrafted) //�ksir craftlanm��sa 
            {
                IfPotionCrafted();
            }
            else //�ksir craftlamam��sa
            {
                potionButtonImage.gameObject.SetActive(false); //�ksir butonu ile ilgili elemanlar� kapat.
                potionEffectBackground.gameObject.SetActive(false);
                potionInfo.gameObject.SetActive(false);
                potionEffectText.gameObject.SetActive(false);
                inventoryButtonInfo.gameObject.SetActive(true); //Envanter info g�rselini a�.
            }

        }




        public void UsePotion() //�ksiri kulland���m�zda ger�ekle�ecek i�lemler.
        {
            potionButton.enabled = false;

            var seq = DOTween.Sequence();

            questsTitleText.text = questTitleDescriptions[1]; //Yap�lan g�rev yaz�s�n� 3/3 olarak g�ncelle

            foreach (var item in quest3CompletedBackground) //3.Questin arkaplan rengini ye�ile �evir ve scale ayar� yap.
            {
                seq.Join(item.DOColor(whiteColor, questCompleteDuration));
                seq.Join(item.transform.DOScale(Vector3.one, questCompleteDuration)).SetEase(Ease.InFlash);
            }
            seq.Join(quest3Tick.DOColor(whiteColor, questCompleteDuration)); //Questin tik i�aretinin rengini belirginle�tir.
            
            seq.Join(potionButtonImage.transform.DOScale(Vector3.one*.8f, questCompleteDuration)); //�ksir butonunun g�rselini k���lt.
            seq.Join(potionButtonImage.DOColor(halfTransparentWhiteColor, questCompleteDuration)).OnStart(()=>
            {
                quest3Text.text = quest3CompletedExplanation;
                potionInfo.gameObject.SetActive(false);
                potionEffectBackground.gameObject.SetActive(false);
                potionEffectText.gameObject.SetActive(false);

            }); //�ksir butonunun sembol�n� solukla�t�r ve yan�ndaki g�rselleri kapat.

            seq.AppendInterval(.7f);
            
            seq.Append(topArea.DOMoveY(topArea.position.y + mainAreasDeflectionValue, mainAreasMovementDuration)); //Ana b�lgeleri ilgili y�nlerde hareket ettir.
            seq.Join(bottomArea.DOMoveY(bottomArea.position.y - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(leftArea.DOMoveX(leftArea.position.x - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(rightArea.DOMoveX(rightArea.position.x + mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(bluredBackground.DOColor(whiteColor, mainAreasMovementDuration)).OnComplete(() => //Bulan�k arkaplan g�r�nt�s�n� belirginle�tir.
            {
                OpenEndScreen();
            }).SetEase(Ease.OutQuad);

            quest3Star.gameObject.SetActive(false); //Quest y�ld�z ikonunu kapat.
            foreach (var item in quest3Background) //Questin siyah arkaplan�n� kapat.
            {
                item.gameObject.SetActive(false);
            }



        }

        private void IfPotionCrafted() //�ksir craftlanm��sa ve ana men�ye tekrar d�nd���m�zde ger�ekle�ecek i�lemler.
        {

            inventoryButton.enabled = false;

            potionButtonImage.gameObject.SetActive(true); //�ksir butonu ile ilgili elemanlar� a�.
            potionEffectBackground.gameObject.SetActive(true);
            potionInfo.gameObject.SetActive(true);
            potionEffectText.gameObject.SetActive(true);
            inventoryButtonInfo.gameObject.SetActive(false); //Envanter info g�rselini kapat.

            var seq = DOTween.Sequence();

            foreach (var item in quest2CompletedBackground) //2.Questin arkaplan rengini ye�ile �evir ve scale ayar� yap.
            {
                seq.Join(item.DOColor(whiteColor, questCompleteDuration));
                seq.Join(item.transform.DOScale(Vector3.one, questCompleteDuration)).SetEase(Ease.InFlash);
            }
            seq.Join(quest2Tick.DOColor(whiteColor, questCompleteDuration)).OnComplete(() =>
            {
                questsTitleText.text = questTitleDescriptions[0]; //Yap�lan g�rev yaz�s�n� 2/3 olarak g�ncelle
            }); //Questin tik i�aretinin rengini belirginle�tir.

            quest2Star.gameObject.SetActive(false); //Quest y�ld�z ikonunu kapat.
            foreach (var item in quest2Background) //Questin siyah arkaplan�n� kapat.
            {
                item.gameObject.SetActive(false);
            }
        }

        public void OnClickInventoryButton() //Envanter butonuna bas�ld���nda ger�ekle�ecek i�lemler.
        {
            var seq = DOTween.Sequence();

            seq.Append(topArea.DOMoveY(topArea.position.y + mainAreasDeflectionValue, mainAreasMovementDuration)).OnStart(()=>
            {
                inventoryButton.image.color = buttonClickedWhiteColor;
                inventoryButtonFrame.color = whiteColor;
            }); //Ana b�lgeleri ilgili y�nlerde hareket ettir.
            seq.Join(bottomArea.DOMoveY(bottomArea.position.y - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(leftArea.DOMoveX(leftArea.position.x - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(rightArea.DOMoveX(rightArea.position.x + mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(bluredBackground.DOColor(whiteColor, mainAreasMovementDuration)).OnComplete(() => //Bulan�k arkaplan g�r�nt�s�n� belirginle�tir.
            {
                inventoryButton.image.color = transparentBlackColor;
                inventoryButtonFrame.color = yellowColor;
                OpenInventoryScreen();
            }).SetEase(Ease.OutQuad);
        }


       
        public async void OpenEndScreen()
        {
            var screenManager = ScreenManager.Instance;

            //await Task.Delay(250);

            await screenManager.OpenScreen(ScreenKeys.EndScreen, ScreenLayerKeys.SecondLayer,false);
        }

        public async void OpenInventoryScreen()
        {
            var screenManager = ScreenManager.Instance;

            await screenManager.OpenScreen(ScreenKeys.GameScreen, ScreenLayerKeys.FirstLayer);
        }


    }
}
