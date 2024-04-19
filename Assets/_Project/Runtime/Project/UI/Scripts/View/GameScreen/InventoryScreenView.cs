using System;
using System.Threading.Tasks;
using _Project.Runtime.Core.Singleton;
using _Project.Runtime.Core.UI.Scripts.Manager;
using _Project.Runtime.Project.Launcher.Scripts.Manager.Bootstrap;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;

namespace _Project.Runtime.Project.UI.Scripts.View
{
    public class InventoryScreenView : SingletonBehaviour<InventoryScreenView>
    {
        [Header("Control Variables")]
        [Space]
        public static bool isPotionCrafted; //Craft iþlemini tamamlayýp tamamlamadýðýmýzý kontrol edeceðimiz deðiþken.

        [Space]
        [Space]
        [Header("Transition Variables")]
        [Space]
        [SerializeField] private float mainAreaDeflectionValue; //Screen ilk açýldýðýnda main arealarýn yukarýdan ve aþaðýdan kaç birim hareket edeceði.
        [SerializeField] private float bigPotionDeflectionValue; //Ýksire týkladýðýmýzda büyük iksir simgesi ve tarif baþlýðýnýn kaç birim yukarýdan hareket edeceðini belirleyen deðiþken.
        [SerializeField] private float lineScaleValue; //Kare sliderlerin çizgilerinin büyüme oraný.
        [SerializeField] private float dropEffectDuration; //Yukarýdan aþaðý doðru hareket edecek olan elementlerin hareket etme süresi.
        [SerializeField] private float mainAreaMovementDuration; //Screen ilk açýldýðýnda main arealarýn hareket süresi.
        [SerializeField] private float potionImageColorSwapDuration; //Büyük iksir ikonlarýnýn renk geçiþ süreleri.
        [SerializeField] private float recipeTextsColorSwapDuration; //Tarif yazýlarýnýn renk geçiþ süreleri.
        [SerializeField] private float buttonColorSwapDuration; //Craft butonunun renk deðiþtirme süresi.
        [SerializeField] private float scaleDuration; //Seçilen eþyanýn (iksir) üzerinde belirecek kýrmýzý çerçeve.
        [SerializeField] private float lineFillingDuration; //Craft sýrasýnda çizgilerin dolma hýzý.
        [SerializeField] private float lineScaleDuration; //Kare sliderlerin büyüme süresi.
        [SerializeField] private float lineColorSwapDuration; //Slider ve dýþ çizgilerinin renklerinin solma süresi.
        [SerializeField] private float materialSlotsColorSwapDuration; //Craft iþleminden sonra dolu slotlarýn ve ember ikonlarýnýn renk geçiþ süreleri.
        [SerializeField] private float delay; //Typewrite effect fonksiyonunda kullanacaðýmýz, harfler arasýnda geçecek süre. 
        [SerializeField] [TextArea] private string description; //Ýksire týkladýðýmýzda tarif kýsmýnda ekranda yazacak yazý.
        [SerializeField] [TextArea] private string buttonText; //Craft butonuna bastýðýmýzda butonda yazacak olan yazý.

        [Space]
        [Space]
        [Header("Colors")]
        [Space]
        [SerializeField] private Color yellowColor; //Projede kullanýlan sarý renk tonu.
        [SerializeField] private Color transparentYellowColor; //Projede kullanýlan alpha deðeri 0 olan sarý renk tonu.
        [SerializeField] private Color whiteColor; //Projede kullanýlan beyaz renk tonu.
        [SerializeField] private Color transparentWhiteColor; //Projede kullanýlan alpha deðeri 0 olan beyaz renk tonu.
        [SerializeField] private Color halfTransparentWhiteColor; //Yarý saydam beyaz renk tonu.
        [SerializeField] private Color greyColor; //Projede kullanýlan beyaz renk tonu.
        [SerializeField] private Color transparentGreyColor; //Projede kullanýlan alpha deðeri 0 gri renk tonu.


        [Space]
        [Space]
        [Header("Images")]
        [Space]
        [SerializeField] private Image[] filledMaterialSlots; //Ýksiri seçtiðimizde materials sekmesinde rengi deðiþecek olan 3 slot
        [SerializeField] private Image[] materialSlotsEmberImages; //Ýksiri seçtiðimizde materials sekmesinde belirecek olan Ember ikonlarý.
        [SerializeField] private Image[] squareSliderLines; //Craft butonuna bastýðýmýzda dolacak kare sliderin çizgileri.
        [SerializeField] private Image[] outerLines; //Sliderlerin dýþ çizgileri.
        [SerializeField] private Image linesParent; //Sliderin çizgileri ve dýþ çizgilerin tamamýnýn parenti.
        [SerializeField] private Image outerLinesParent; //Sliderin dýþ çizgilerinin parenti.
        [SerializeField] private Image selectedItemImage; //Ýksiri seçtiðimizde yanýp sönen kýrmýzý çerçeve.
        [SerializeField] private Image recipeTextFrame; //Tarif yazýsýnýn yazacaðý kýsmýn çerçevesi.
        [SerializeField] private Image bigPotionImage; //Ýksire týkladýðýmýzda belirecek büyük iksir ikonu.
        [SerializeField] private Image bigPotionEmptyImage; //Ýksire týkladýðýmýzda kaybolacak ikon.
        [SerializeField] private Image bigPotionFrame; //Ýksire týkladýðýmýzda renk deðiþtirecek çerçeve.
        [SerializeField] private Image craftButtonDisable; //Ýksir seçilmemiþse aktif olacak gri renkli craft butonu.
        [SerializeField] private Image craftButtonEnable; //Ýksir seçilmiþse aktif olacak turuncu renkli craft butonu.
        [SerializeField] private Image inventoryEmberIcon; //Envanterdeki ember ikonu.
        [SerializeField] private Image inventoryEmberCircle; //Envanterdeki ember ikonunun yanýndaki turuncu daire.


        [Space]
        [Space]
        [Header("Texts")]
        [Space]
        [SerializeField] private TextMeshProUGUI selectRecipeText; //Baþlangýçta yazacak olan yazý. Ýksire týklayýnca kaybolacak.
        [SerializeField] private TextMeshProUGUI recipeHeaderText; //Ýksire týkladýktan sonra ekranda yazacak olan tarif baþlýðý. 
        [SerializeField] private TextMeshProUGUI recipeDescriptionText; //Ýksire týkladýktan sonra ekranda yazacak olan tarif metni.
        [SerializeField] private TextMeshProUGUI disableCraftButtonText; //Craft öncesi ve sonrasý aktif olacak buton yazýsý. (Craft)
        [SerializeField] private TextMeshProUGUI disableInCraftingButtonText; // Craft sýrasýnda aktif olacak buton yazýsý. (In crafting)
        [SerializeField] private TextMeshProUGUI inventoryEmberText; //Envanterdeki ember ikonunun yanýndaki text.

        [Space]
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button craftButton; //Turuncu craft butonu.
        [SerializeField] private Button selectPotionButton; //Ýksirin üzerindeki buton.

        [Space]
        [Space]
        [Header("MainAreas")]
        [SerializeField] private RectTransform topArea; //Ekranýn ortasýndan yukarýsý.
        [SerializeField] private RectTransform bottomArea; //Ekranýn ortasýndan aþaðýsý.

        [Space]
        [Space]
        [Header("Other")]
        [SerializeField] private Ease scaleEase; //Kare sliderin ve dýþ çizgilerinin scale easei.
        [SerializeField] private TextMeshProUGUI[] allTexts; //Screen kapatýlýrken tüm textleri transparanlaþtýracaðýmýz liste.
        [SerializeField] private Image[] allImagess; //Screen kapatýlýrken tüm imageleri transparanlaþtýracaðýmýz liste.

        private void Awake()
        {
            allTexts = FindObjectsOfType<TextMeshProUGUI>();
            allImagess = FindObjectsOfType<Image>();
        }

        private void Start()
        {
            SetStartStatus();
            ScreenOpenningAnimations();
        }

        public void OnClickedCraftButton() //Craft butonuna bastýðýmýzda gerçekleþecek iþlemler.
        {
            var seq = DOTween.Sequence();

            craftButtonEnable.gameObject.SetActive(false); //Turuncu craft butonunu gizle.
            craftButtonDisable.gameObject.SetActive(true); //Gri craft butonunu aktif et.
            disableCraftButtonText.gameObject.SetActive(false); //Gri craft butonundaki craft yazýsýný kapat.
            disableInCraftingButtonText.gameObject.SetActive(true); //Gri craft butonundaki in craft yazýsýný aç.

            for (int i = 0; i < squareSliderLines.Length; i++)
            {
                seq.Append(squareSliderLines[i].DOFillAmount(1, lineFillingDuration)).SetEase(Ease.Linear).OnStart(() =>
                {
                    StartCoroutine(ButtonTextTypeWriteEffect()); //In craft yazýsýný yazdýr.
                });//Craft butonuna basýldýðýnda kare sliderin çizgilerini teker teker doldur. 
            }

            seq.Append(linesParent.transform.DOScale(Vector3.one * lineScaleValue, lineScaleDuration).SetEase(scaleEase).OnStart(() =>
            {
                disableInCraftingButtonText.gameObject.SetActive(false); //Slider dolduktan sonra in craft yazýsýný kapat.
                disableCraftButtonText.gameObject.SetActive(true); //In craft yazýsý kapandýktan sonra, tekrar craft yazýsýný aç.
                outerLinesParent.gameObject.SetActive(true); //Dýþ çizgileri aktif et.

                for (int i = 0; i < filledMaterialSlots.Length; i++) //Craft iþlemi sonrasý dolu material slotlarýndaki deðiþiklikler.
                {
                    filledMaterialSlots[i].DOColor(greyColor, materialSlotsColorSwapDuration); //Slotun rengini griye çevir.
                    materialSlotsEmberImages[i].DOColor(halfTransparentWhiteColor, materialSlotsColorSwapDuration); //Ember ikonunu bir miktar soldur.
                }

                var sequance = DOTween.Sequence(); //Craftlama iþlemi bittikten sonra envanter slotunda gerçekleþecek iþlemler.

                sequance.Join(inventoryEmberIcon.transform.DOMoveY(inventoryEmberIcon.transform.position.y + bigPotionDeflectionValue, dropEffectDuration));
                sequance.Join(inventoryEmberIcon.DOColor(transparentWhiteColor, dropEffectDuration));
                sequance.Join(inventoryEmberCircle.transform.DOMoveY(inventoryEmberCircle.transform.position.y + bigPotionDeflectionValue, dropEffectDuration));
                sequance.Join(inventoryEmberCircle.DOColor(transparentWhiteColor, dropEffectDuration));
                sequance.Join(inventoryEmberText.transform.DOMoveY(inventoryEmberText.transform.position.y + bigPotionDeflectionValue, dropEffectDuration));
                sequance.Join(inventoryEmberText.DOColor(transparentWhiteColor, dropEffectDuration)).OnComplete(() =>
                {
                    inventoryEmberCircle.gameObject.SetActive(false);
                    inventoryEmberIcon.gameObject.SetActive(false);
                    inventoryEmberText.gameObject.SetActive(false);
                    
                    isPotionCrafted = true;
                });

            })); //Slider çizgilerini ve dýþ çizgilerini belirtilen miktarda büyüt. Bu sýrada dýþ çizgileri aktif et.

            for (int i = 0; i < squareSliderLines.Length; i++) //Slider çizgilerini ve dýþ çizgilerini yavaþ yavaþ soldur.
            {
                seq.Join(squareSliderLines[i].DOColor(transparentYellowColor, lineColorSwapDuration));
                seq.Join(outerLines[i].DOColor(transparentYellowColor, lineColorSwapDuration));
            }
        }
        public void OnClickedToThePotion() //Ýksire týkladýðýmýzda çalýþacak fonksiyon.
        {
            selectPotionButton.interactable = false; //Butona 1 kere basýldýktan sonra birdaha basýlmasýný engelle.
            var seq = DOTween.Sequence();
            selectedItemImage.gameObject.SetActive(true);
            selectedItemImage.transform.DOScale(Vector3.one * 1.08f, scaleDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            seq.Append(bigPotionEmptyImage.DOColor(transparentGreyColor, potionImageColorSwapDuration)); //Boþ iksiri ikonunu transparan yap.
            seq.Join(bigPotionImage.DOColor(whiteColor, potionImageColorSwapDuration)); //Ýksir ikonunu görünür yap.
            seq.Join(selectRecipeText.DOColor(transparentGreyColor, recipeTextsColorSwapDuration)); //Select Recipe yazýsýný transparan yap.
            seq.Join(recipeHeaderText.DOColor(yellowColor, recipeTextsColorSwapDuration).OnComplete(() =>
            {
                recipeDescriptionText.gameObject.SetActive(true);
                StartCoroutine(RecipeTypeWriteEffect());
            })); //Tarif baþlýðýný görünür yap ve ardýndan daktilo efektini gerçekleþtir.
            seq.Join(craftButtonEnable.DOColor(whiteColor, buttonColorSwapDuration)).OnStart(() => //Turuncu butonu aç, gri butonu kapat ve turuncu butonun rengini yavaþça görünür yap.
            {
                for (int i = 0; i < filledMaterialSlots.Length; i++) //Ýksire týkladýktan sonra material slotundaki deðiþiklikler.
                {
                    materialSlotsEmberImages[i].transform.position += Vector3.up * bigPotionDeflectionValue; //Drop effect için ember ikonlarýný bir miktar yukarý hareket ettir.
                    filledMaterialSlots[i].DOColor(yellowColor, 0f); //Yazma iþlemi bittikten sonra materials sekmesindeki 3 adet slotun rengini deðiþtir.
                    materialSlotsEmberImages[i].gameObject.SetActive(true); //Yazma iþlemi bittikten sonra materials sekmesindeki 3 adet ikon aktif olsun.
                    materialSlotsEmberImages[i].transform.DOMoveY(materialSlotsEmberImages[i].transform.position.y - bigPotionDeflectionValue, dropEffectDuration); //Ember ikonlarýn drop effect ver.
                }

                craftButtonDisable.gameObject.SetActive(false);
                craftButtonEnable.gameObject.SetActive(true);
            });
            seq.Join(recipeTextFrame.DOColor(yellowColor, potionImageColorSwapDuration)); //Tarif kutucuðunun rengini sarýya çevir.
            seq.Join(bigPotionFrame.DOColor(yellowColor, potionImageColorSwapDuration)); //Büyük iksir slotunun çerçevesinin rengini sarýya çevir.
            seq.Join(bigPotionImage.transform.DOMoveY(bigPotionImage.transform.position.y - bigPotionDeflectionValue, dropEffectDuration)); //Büyük iksir sembolünü yukarýdan yerine doðru hareket ettir.
            seq.Join(recipeHeaderText.transform.DOMoveY(recipeHeaderText.transform.position.y - bigPotionDeflectionValue, dropEffectDuration)); //Tarif baþlýðýný yukarýdan yerine doðru hareket ettir.

        }

        private void SetStartStatus() //Screen ilk açýldýðýnda gerçekleþecek iþlemler.
        {
            selectedItemImage.gameObject.SetActive(false); //Kýrmýzý çerçeve baþlangýçta kapalý olacak. Ýksiri seçtiðimizde açýlacak.
            recipeHeaderText.color = transparentYellowColor; //Tarif baþlýðýnýn rengini baþlangýçta transparan yap.
            recipeDescriptionText.gameObject.SetActive(false); //Ýksir içerik baþlýðý baþlangýçta kapalý olsun.

            foreach (var item in materialSlotsEmberImages) //Baþlangýçta materials sekmesindeki ember ikonlarýný gizle.
            {
                item.gameObject.SetActive(false);
            }

            outerLinesParent.gameObject.SetActive(false);

            craftButtonEnable.gameObject.SetActive(false); //Baþlangýçta iksir seçilmeyeceði için turuncu craft butonunu deaktif et.
            disableInCraftingButtonText.gameObject.SetActive(false); //"In crafting" ibaresini baþlangýçta kapat.

            bigPotionImage.transform.position += Vector3.up * bigPotionDeflectionValue; //Ýksir simgesini bir miktar yukarý hareket ettir.
            recipeHeaderText.transform.position += Vector3.up * bigPotionDeflectionValue; //Tarif baþlýðýný bir miktar yukarý hareket ettir.
        }

        private void ScreenOpenningAnimations() //Screen ilk açýldýðýnda gerçekleþecek animasyon.
        {
            topArea.transform.position += Vector3.up * mainAreaDeflectionValue;
            bottomArea.transform.position += Vector3.down * mainAreaDeflectionValue;
            var seq = DOTween.Sequence();

            seq.Append(topArea.transform.DOMoveY(topArea.transform.position.y - mainAreaDeflectionValue, mainAreaMovementDuration));
            seq.Join(bottomArea.transform.DOMoveY(bottomArea.transform.position.y + mainAreaDeflectionValue, mainAreaMovementDuration));
        }
        public void OnClickBackButton() //Geri tuþuna bastýðýmýzda gerçekleþecek iþlemler.
        {

            if (isPotionCrafted) //Craft iþlemini tamamlamadan geri dönülmesin.
            {
                topArea.DOMoveY(topArea.position.y + mainAreaDeflectionValue, mainAreaMovementDuration).OnStart(()=>
                {
                    foreach (var item in allImagess)
                    {
                        item.DOColor(transparentWhiteColor, mainAreaMovementDuration);
                    }

                    foreach (var item in allTexts)
                    {
                        item.DOColor(transparentWhiteColor, mainAreaMovementDuration);
                    }
                }).OnComplete(() =>
                {
                    OpenMenuScreen();
                }); //Alt ve üst bölgeyi ilgili yöne doðru hareket ettir, bu sýrada canvastaki tüm text ve image bileþenlerinin opasitesini düþür. Ýþlem bittiðinde ilgili screeni yükle.
                bottomArea.DOMoveY(bottomArea.position.y - mainAreaDeflectionValue, mainAreaMovementDuration);    
            }
        }

        private async void OpenMenuScreen() //Ana menü screenini yüklediðimiz fonksiyon.
        {
            var screenManager = ScreenManager.Instance;
            await screenManager.OpenScreen(ScreenKeys.MenuScreen, ScreenLayerKeys.FirstLayer);
        }

        IEnumerator ButtonTextTypeWriteEffect() //Craft sýrasýnda butonda yazacak yazýyý daktilo efekti ile yazdýrmamýzý saðlayan coroutine.
        {
            foreach (char item in buttonText)
            {
                disableInCraftingButtonText.text += item.ToString();
                yield return new WaitForSeconds((lineFillingDuration * .5f) / buttonText.Length);
            }
        }
        IEnumerator RecipeTypeWriteEffect() //Ýksire týkladýðýmýzda tarif kýsmýnda yazacak yazýyý daktilo efekti ile yazdýrmamýzý saðlayan coroutine.
        {
            foreach (char item in description)
            {
                recipeDescriptionText.text += item.ToString();

                if (item == ' ')
                {
                    yield return new WaitForSeconds(delay * 1.2f);
                }
                else
                {
                    yield return new WaitForSeconds(delay);
                }
            }

            craftButton.interactable = true; //Yazma iþlemi bittikten sonra craf butonuna týklayabilirliði aç.
        }


    }
}
