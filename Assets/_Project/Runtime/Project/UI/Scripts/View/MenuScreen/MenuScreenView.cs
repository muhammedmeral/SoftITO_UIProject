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
        [SerializeField] private float mainAreasDeflectionValue; //Ana kýsýmlarýn kaç birim hareket edeceði.
        [SerializeField] private float mainAreasMovementDuration; //Ana birimlerin hareket süresi.
        [SerializeField] private float questCompleteDuration; //Quest tamamlandýðýnda renk deðiþiminin süresi.
        [SerializeField] private string[] questTitleDescriptions; //Quest baþlýðýnda yazacak yazýlar.
        [SerializeField] private string quest3CompletedExplanation; //Quest3 tamamlandýðýnda yazacak yazý.

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
        [SerializeField] private Image[] quest2CompletedBackground; //Quest2 tamamlandýðýnda aktif olacak yeþil arkaplan.
        [SerializeField] private Image[] quest2Background; //Quest2 tamamlanmamýþsa aktif olacak arkaplan.
        [SerializeField] private Image[] quest3CompletedBackground; //Quest3 tamamlandýðýnda aktif olacak yeþil arkaplan.
        [SerializeField] private Image[] quest3Background; //Quest3 tamamlanmamýþsa aktif olacak arkaplan.
        [SerializeField] private Image bluredBackground; //Sahne geçiþi sýrasýnda kullanacaðýmýz bulanýk arkaplan.
        [SerializeField] private Image potionButtonImage; //Ýksir butonunun image componenti.
        [SerializeField] private Image potionEffectBackground; //Ýksir butonunun etki yazýsýnýn arkaplaný.
        [SerializeField] private Image potionInfo; //Ýksir butonun yanýnda görünecek info butonu.
        [SerializeField] private Image inventoryButtonInfo; //Envanter butonun yanýnda görünen info görseli.        
        [SerializeField] private Image inventoryButtonFrame; //Envanter butonu görseli.        
        [SerializeField] private Image quest2Tick; //Qutes2 tamamlandýðýnda aktif olacak tik iþareti.
        [SerializeField] private Image quest2Star; //Quest2 tamamlanmadýðýnda aktif olacak yýldýz iþareti.
        [SerializeField] private Image quest3Tick; //Quest3 tamamlandýðýnda aktif olacak tik iþareti.
        [SerializeField] private Image quest3Star; //Quest3 tamamlanmadýðýnda aktif olacak yýldýz iþareti.
        [SerializeField] private Image consumableSlot;

        [Space]
        [Space]
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI potionEffectText; //Ýksir butonun etki yazýsý.
        [SerializeField] private TextMeshProUGUI questsTitleText; //Quest baþlýðý.
        [SerializeField] private TextMeshProUGUI quest3Text; //Quest3'ün açýklama yazýsý.

        [Space]
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button inventoryButton; //Envanter butonu.
        [SerializeField] private Button potionButton; //Ýksir butonu

        [Space]
        [Space]
        [Header("Main Areas")]
        [Space]
        [SerializeField] private RectTransform topArea; //Üst ana bölge.
        [SerializeField] private RectTransform bottomArea; //Alt ana bölge.
        [SerializeField] private RectTransform rightArea; //Sað ana bölge.
        [SerializeField] private RectTransform leftArea; //Sol ana bölge.

        private void Start()
        {
            if (InventoryScreenView.isPotionCrafted) //Ýksir craftlanmýþsa 
            {
                IfPotionCrafted();
            }
            else //Ýksir craftlamamýþsa
            {
                potionButtonImage.gameObject.SetActive(false); //Ýksir butonu ile ilgili elemanlarý kapat.
                potionEffectBackground.gameObject.SetActive(false);
                potionInfo.gameObject.SetActive(false);
                potionEffectText.gameObject.SetActive(false);
                inventoryButtonInfo.gameObject.SetActive(true); //Envanter info görselini aç.
            }

        }




        public void UsePotion() //Ýksiri kullandýðýmýzda gerçekleþecek iþlemler.
        {
            potionButton.enabled = false;

            var seq = DOTween.Sequence();

            questsTitleText.text = questTitleDescriptions[1]; //Yapýlan görev yazýsýný 3/3 olarak güncelle

            foreach (var item in quest3CompletedBackground) //3.Questin arkaplan rengini yeþile çevir ve scale ayarý yap.
            {
                seq.Join(item.DOColor(whiteColor, questCompleteDuration));
                seq.Join(item.transform.DOScale(Vector3.one, questCompleteDuration)).SetEase(Ease.InFlash);
            }
            seq.Join(quest3Tick.DOColor(whiteColor, questCompleteDuration)); //Questin tik iþaretinin rengini belirginleþtir.
            
            seq.Join(potionButtonImage.transform.DOScale(Vector3.one*.8f, questCompleteDuration)); //Ýksir butonunun görselini küçült.
            seq.Join(potionButtonImage.DOColor(halfTransparentWhiteColor, questCompleteDuration)).OnStart(()=>
            {
                quest3Text.text = quest3CompletedExplanation;
                potionInfo.gameObject.SetActive(false);
                potionEffectBackground.gameObject.SetActive(false);
                potionEffectText.gameObject.SetActive(false);

            }); //Ýksir butonunun sembolünü soluklaþtýr ve yanýndaki görselleri kapat.

            seq.AppendInterval(.7f);
            
            seq.Append(topArea.DOMoveY(topArea.position.y + mainAreasDeflectionValue, mainAreasMovementDuration)); //Ana bölgeleri ilgili yönlerde hareket ettir.
            seq.Join(bottomArea.DOMoveY(bottomArea.position.y - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(leftArea.DOMoveX(leftArea.position.x - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(rightArea.DOMoveX(rightArea.position.x + mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(bluredBackground.DOColor(whiteColor, mainAreasMovementDuration)).OnComplete(() => //Bulanýk arkaplan görüntüsünü belirginleþtir.
            {
                OpenEndScreen();
            }).SetEase(Ease.OutQuad);

            quest3Star.gameObject.SetActive(false); //Quest yýldýz ikonunu kapat.
            foreach (var item in quest3Background) //Questin siyah arkaplanýný kapat.
            {
                item.gameObject.SetActive(false);
            }



        }

        private void IfPotionCrafted() //Ýksir craftlanmýþsa ve ana menüye tekrar döndüðümüzde gerçekleþecek iþlemler.
        {

            inventoryButton.enabled = false;

            potionButtonImage.gameObject.SetActive(true); //Ýksir butonu ile ilgili elemanlarý aç.
            potionEffectBackground.gameObject.SetActive(true);
            potionInfo.gameObject.SetActive(true);
            potionEffectText.gameObject.SetActive(true);
            inventoryButtonInfo.gameObject.SetActive(false); //Envanter info görselini kapat.

            var seq = DOTween.Sequence();

            foreach (var item in quest2CompletedBackground) //2.Questin arkaplan rengini yeþile çevir ve scale ayarý yap.
            {
                seq.Join(item.DOColor(whiteColor, questCompleteDuration));
                seq.Join(item.transform.DOScale(Vector3.one, questCompleteDuration)).SetEase(Ease.InFlash);
            }
            seq.Join(quest2Tick.DOColor(whiteColor, questCompleteDuration)).OnComplete(() =>
            {
                questsTitleText.text = questTitleDescriptions[0]; //Yapýlan görev yazýsýný 2/3 olarak güncelle
            }); //Questin tik iþaretinin rengini belirginleþtir.

            quest2Star.gameObject.SetActive(false); //Quest yýldýz ikonunu kapat.
            foreach (var item in quest2Background) //Questin siyah arkaplanýný kapat.
            {
                item.gameObject.SetActive(false);
            }
        }

        public void OnClickInventoryButton() //Envanter butonuna basýldýðýnda gerçekleþecek iþlemler.
        {
            var seq = DOTween.Sequence();

            seq.Append(topArea.DOMoveY(topArea.position.y + mainAreasDeflectionValue, mainAreasMovementDuration)).OnStart(()=>
            {
                inventoryButton.image.color = buttonClickedWhiteColor;
                inventoryButtonFrame.color = whiteColor;
            }); //Ana bölgeleri ilgili yönlerde hareket ettir.
            seq.Join(bottomArea.DOMoveY(bottomArea.position.y - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(leftArea.DOMoveX(leftArea.position.x - mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(rightArea.DOMoveX(rightArea.position.x + mainAreasDeflectionValue, mainAreasMovementDuration));
            seq.Join(bluredBackground.DOColor(whiteColor, mainAreasMovementDuration)).OnComplete(() => //Bulanýk arkaplan görüntüsünü belirginleþtir.
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
