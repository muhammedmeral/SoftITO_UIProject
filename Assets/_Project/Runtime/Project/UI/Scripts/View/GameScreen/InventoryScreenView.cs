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
        public static bool isPotionCrafted; //Craft i�lemini tamamlay�p tamamlamad���m�z� kontrol edece�imiz de�i�ken.

        [Space]
        [Space]
        [Header("Transition Variables")]
        [Space]
        [SerializeField] private float mainAreaDeflectionValue; //Screen ilk a��ld���nda main arealar�n yukar�dan ve a�a��dan ka� birim hareket edece�i.
        [SerializeField] private float bigPotionDeflectionValue; //�ksire t�klad���m�zda b�y�k iksir simgesi ve tarif ba�l���n�n ka� birim yukar�dan hareket edece�ini belirleyen de�i�ken.
        [SerializeField] private float lineScaleValue; //Kare sliderlerin �izgilerinin b�y�me oran�.
        [SerializeField] private float dropEffectDuration; //Yukar�dan a�a�� do�ru hareket edecek olan elementlerin hareket etme s�resi.
        [SerializeField] private float mainAreaMovementDuration; //Screen ilk a��ld���nda main arealar�n hareket s�resi.
        [SerializeField] private float potionImageColorSwapDuration; //B�y�k iksir ikonlar�n�n renk ge�i� s�releri.
        [SerializeField] private float recipeTextsColorSwapDuration; //Tarif yaz�lar�n�n renk ge�i� s�releri.
        [SerializeField] private float buttonColorSwapDuration; //Craft butonunun renk de�i�tirme s�resi.
        [SerializeField] private float scaleDuration; //Se�ilen e�yan�n (iksir) �zerinde belirecek k�rm�z� �er�eve.
        [SerializeField] private float lineFillingDuration; //Craft s�ras�nda �izgilerin dolma h�z�.
        [SerializeField] private float lineScaleDuration; //Kare sliderlerin b�y�me s�resi.
        [SerializeField] private float lineColorSwapDuration; //Slider ve d�� �izgilerinin renklerinin solma s�resi.
        [SerializeField] private float materialSlotsColorSwapDuration; //Craft i�leminden sonra dolu slotlar�n ve ember ikonlar�n�n renk ge�i� s�releri.
        [SerializeField] private float delay; //Typewrite effect fonksiyonunda kullanaca��m�z, harfler aras�nda ge�ecek s�re. 
        [SerializeField] [TextArea] private string description; //�ksire t�klad���m�zda tarif k�sm�nda ekranda yazacak yaz�.
        [SerializeField] [TextArea] private string buttonText; //Craft butonuna bast���m�zda butonda yazacak olan yaz�.

        [Space]
        [Space]
        [Header("Colors")]
        [Space]
        [SerializeField] private Color yellowColor; //Projede kullan�lan sar� renk tonu.
        [SerializeField] private Color transparentYellowColor; //Projede kullan�lan alpha de�eri 0 olan sar� renk tonu.
        [SerializeField] private Color whiteColor; //Projede kullan�lan beyaz renk tonu.
        [SerializeField] private Color transparentWhiteColor; //Projede kullan�lan alpha de�eri 0 olan beyaz renk tonu.
        [SerializeField] private Color halfTransparentWhiteColor; //Yar� saydam beyaz renk tonu.
        [SerializeField] private Color greyColor; //Projede kullan�lan beyaz renk tonu.
        [SerializeField] private Color transparentGreyColor; //Projede kullan�lan alpha de�eri 0 gri renk tonu.


        [Space]
        [Space]
        [Header("Images")]
        [Space]
        [SerializeField] private Image[] filledMaterialSlots; //�ksiri se�ti�imizde materials sekmesinde rengi de�i�ecek olan 3 slot
        [SerializeField] private Image[] materialSlotsEmberImages; //�ksiri se�ti�imizde materials sekmesinde belirecek olan Ember ikonlar�.
        [SerializeField] private Image[] squareSliderLines; //Craft butonuna bast���m�zda dolacak kare sliderin �izgileri.
        [SerializeField] private Image[] outerLines; //Sliderlerin d�� �izgileri.
        [SerializeField] private Image linesParent; //Sliderin �izgileri ve d�� �izgilerin tamam�n�n parenti.
        [SerializeField] private Image outerLinesParent; //Sliderin d�� �izgilerinin parenti.
        [SerializeField] private Image selectedItemImage; //�ksiri se�ti�imizde yan�p s�nen k�rm�z� �er�eve.
        [SerializeField] private Image recipeTextFrame; //Tarif yaz�s�n�n yazaca�� k�sm�n �er�evesi.
        [SerializeField] private Image bigPotionImage; //�ksire t�klad���m�zda belirecek b�y�k iksir ikonu.
        [SerializeField] private Image bigPotionEmptyImage; //�ksire t�klad���m�zda kaybolacak ikon.
        [SerializeField] private Image bigPotionFrame; //�ksire t�klad���m�zda renk de�i�tirecek �er�eve.
        [SerializeField] private Image craftButtonDisable; //�ksir se�ilmemi�se aktif olacak gri renkli craft butonu.
        [SerializeField] private Image craftButtonEnable; //�ksir se�ilmi�se aktif olacak turuncu renkli craft butonu.
        [SerializeField] private Image inventoryEmberIcon; //Envanterdeki ember ikonu.
        [SerializeField] private Image inventoryEmberCircle; //Envanterdeki ember ikonunun yan�ndaki turuncu daire.


        [Space]
        [Space]
        [Header("Texts")]
        [Space]
        [SerializeField] private TextMeshProUGUI selectRecipeText; //Ba�lang��ta yazacak olan yaz�. �ksire t�klay�nca kaybolacak.
        [SerializeField] private TextMeshProUGUI recipeHeaderText; //�ksire t�klad�ktan sonra ekranda yazacak olan tarif ba�l���. 
        [SerializeField] private TextMeshProUGUI recipeDescriptionText; //�ksire t�klad�ktan sonra ekranda yazacak olan tarif metni.
        [SerializeField] private TextMeshProUGUI disableCraftButtonText; //Craft �ncesi ve sonras� aktif olacak buton yaz�s�. (Craft)
        [SerializeField] private TextMeshProUGUI disableInCraftingButtonText; // Craft s�ras�nda aktif olacak buton yaz�s�. (In crafting)
        [SerializeField] private TextMeshProUGUI inventoryEmberText; //Envanterdeki ember ikonunun yan�ndaki text.

        [Space]
        [Space]
        [Header("Buttons")]
        [SerializeField] private Button craftButton; //Turuncu craft butonu.
        [SerializeField] private Button selectPotionButton; //�ksirin �zerindeki buton.

        [Space]
        [Space]
        [Header("MainAreas")]
        [SerializeField] private RectTransform topArea; //Ekran�n ortas�ndan yukar�s�.
        [SerializeField] private RectTransform bottomArea; //Ekran�n ortas�ndan a�a��s�.

        [Space]
        [Space]
        [Header("Other")]
        [SerializeField] private Ease scaleEase; //Kare sliderin ve d�� �izgilerinin scale easei.
        [SerializeField] private TextMeshProUGUI[] allTexts; //Screen kapat�l�rken t�m textleri transparanla�t�raca��m�z liste.
        [SerializeField] private Image[] allImagess; //Screen kapat�l�rken t�m imageleri transparanla�t�raca��m�z liste.

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

        public void OnClickedCraftButton() //Craft butonuna bast���m�zda ger�ekle�ecek i�lemler.
        {
            var seq = DOTween.Sequence();

            craftButtonEnable.gameObject.SetActive(false); //Turuncu craft butonunu gizle.
            craftButtonDisable.gameObject.SetActive(true); //Gri craft butonunu aktif et.
            disableCraftButtonText.gameObject.SetActive(false); //Gri craft butonundaki craft yaz�s�n� kapat.
            disableInCraftingButtonText.gameObject.SetActive(true); //Gri craft butonundaki in craft yaz�s�n� a�.

            for (int i = 0; i < squareSliderLines.Length; i++)
            {
                seq.Append(squareSliderLines[i].DOFillAmount(1, lineFillingDuration)).SetEase(Ease.Linear).OnStart(() =>
                {
                    StartCoroutine(ButtonTextTypeWriteEffect()); //In craft yaz�s�n� yazd�r.
                });//Craft butonuna bas�ld���nda kare sliderin �izgilerini teker teker doldur. 
            }

            seq.Append(linesParent.transform.DOScale(Vector3.one * lineScaleValue, lineScaleDuration).SetEase(scaleEase).OnStart(() =>
            {
                disableInCraftingButtonText.gameObject.SetActive(false); //Slider dolduktan sonra in craft yaz�s�n� kapat.
                disableCraftButtonText.gameObject.SetActive(true); //In craft yaz�s� kapand�ktan sonra, tekrar craft yaz�s�n� a�.
                outerLinesParent.gameObject.SetActive(true); //D�� �izgileri aktif et.

                for (int i = 0; i < filledMaterialSlots.Length; i++) //Craft i�lemi sonras� dolu material slotlar�ndaki de�i�iklikler.
                {
                    filledMaterialSlots[i].DOColor(greyColor, materialSlotsColorSwapDuration); //Slotun rengini griye �evir.
                    materialSlotsEmberImages[i].DOColor(halfTransparentWhiteColor, materialSlotsColorSwapDuration); //Ember ikonunu bir miktar soldur.
                }

                var sequance = DOTween.Sequence(); //Craftlama i�lemi bittikten sonra envanter slotunda ger�ekle�ecek i�lemler.

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

            })); //Slider �izgilerini ve d�� �izgilerini belirtilen miktarda b�y�t. Bu s�rada d�� �izgileri aktif et.

            for (int i = 0; i < squareSliderLines.Length; i++) //Slider �izgilerini ve d�� �izgilerini yava� yava� soldur.
            {
                seq.Join(squareSliderLines[i].DOColor(transparentYellowColor, lineColorSwapDuration));
                seq.Join(outerLines[i].DOColor(transparentYellowColor, lineColorSwapDuration));
            }
        }
        public void OnClickedToThePotion() //�ksire t�klad���m�zda �al��acak fonksiyon.
        {
            selectPotionButton.interactable = false; //Butona 1 kere bas�ld�ktan sonra birdaha bas�lmas�n� engelle.
            var seq = DOTween.Sequence();
            selectedItemImage.gameObject.SetActive(true);
            selectedItemImage.transform.DOScale(Vector3.one * 1.08f, scaleDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            seq.Append(bigPotionEmptyImage.DOColor(transparentGreyColor, potionImageColorSwapDuration)); //Bo� iksiri ikonunu transparan yap.
            seq.Join(bigPotionImage.DOColor(whiteColor, potionImageColorSwapDuration)); //�ksir ikonunu g�r�n�r yap.
            seq.Join(selectRecipeText.DOColor(transparentGreyColor, recipeTextsColorSwapDuration)); //Select Recipe yaz�s�n� transparan yap.
            seq.Join(recipeHeaderText.DOColor(yellowColor, recipeTextsColorSwapDuration).OnComplete(() =>
            {
                recipeDescriptionText.gameObject.SetActive(true);
                StartCoroutine(RecipeTypeWriteEffect());
            })); //Tarif ba�l���n� g�r�n�r yap ve ard�ndan daktilo efektini ger�ekle�tir.
            seq.Join(craftButtonEnable.DOColor(whiteColor, buttonColorSwapDuration)).OnStart(() => //Turuncu butonu a�, gri butonu kapat ve turuncu butonun rengini yava��a g�r�n�r yap.
            {
                for (int i = 0; i < filledMaterialSlots.Length; i++) //�ksire t�klad�ktan sonra material slotundaki de�i�iklikler.
                {
                    materialSlotsEmberImages[i].transform.position += Vector3.up * bigPotionDeflectionValue; //Drop effect i�in ember ikonlar�n� bir miktar yukar� hareket ettir.
                    filledMaterialSlots[i].DOColor(yellowColor, 0f); //Yazma i�lemi bittikten sonra materials sekmesindeki 3 adet slotun rengini de�i�tir.
                    materialSlotsEmberImages[i].gameObject.SetActive(true); //Yazma i�lemi bittikten sonra materials sekmesindeki 3 adet ikon aktif olsun.
                    materialSlotsEmberImages[i].transform.DOMoveY(materialSlotsEmberImages[i].transform.position.y - bigPotionDeflectionValue, dropEffectDuration); //Ember ikonlar�n drop effect ver.
                }

                craftButtonDisable.gameObject.SetActive(false);
                craftButtonEnable.gameObject.SetActive(true);
            });
            seq.Join(recipeTextFrame.DOColor(yellowColor, potionImageColorSwapDuration)); //Tarif kutucu�unun rengini sar�ya �evir.
            seq.Join(bigPotionFrame.DOColor(yellowColor, potionImageColorSwapDuration)); //B�y�k iksir slotunun �er�evesinin rengini sar�ya �evir.
            seq.Join(bigPotionImage.transform.DOMoveY(bigPotionImage.transform.position.y - bigPotionDeflectionValue, dropEffectDuration)); //B�y�k iksir sembol�n� yukar�dan yerine do�ru hareket ettir.
            seq.Join(recipeHeaderText.transform.DOMoveY(recipeHeaderText.transform.position.y - bigPotionDeflectionValue, dropEffectDuration)); //Tarif ba�l���n� yukar�dan yerine do�ru hareket ettir.

        }

        private void SetStartStatus() //Screen ilk a��ld���nda ger�ekle�ecek i�lemler.
        {
            selectedItemImage.gameObject.SetActive(false); //K�rm�z� �er�eve ba�lang��ta kapal� olacak. �ksiri se�ti�imizde a��lacak.
            recipeHeaderText.color = transparentYellowColor; //Tarif ba�l���n�n rengini ba�lang��ta transparan yap.
            recipeDescriptionText.gameObject.SetActive(false); //�ksir i�erik ba�l��� ba�lang��ta kapal� olsun.

            foreach (var item in materialSlotsEmberImages) //Ba�lang��ta materials sekmesindeki ember ikonlar�n� gizle.
            {
                item.gameObject.SetActive(false);
            }

            outerLinesParent.gameObject.SetActive(false);

            craftButtonEnable.gameObject.SetActive(false); //Ba�lang��ta iksir se�ilmeyece�i i�in turuncu craft butonunu deaktif et.
            disableInCraftingButtonText.gameObject.SetActive(false); //"In crafting" ibaresini ba�lang��ta kapat.

            bigPotionImage.transform.position += Vector3.up * bigPotionDeflectionValue; //�ksir simgesini bir miktar yukar� hareket ettir.
            recipeHeaderText.transform.position += Vector3.up * bigPotionDeflectionValue; //Tarif ba�l���n� bir miktar yukar� hareket ettir.
        }

        private void ScreenOpenningAnimations() //Screen ilk a��ld���nda ger�ekle�ecek animasyon.
        {
            topArea.transform.position += Vector3.up * mainAreaDeflectionValue;
            bottomArea.transform.position += Vector3.down * mainAreaDeflectionValue;
            var seq = DOTween.Sequence();

            seq.Append(topArea.transform.DOMoveY(topArea.transform.position.y - mainAreaDeflectionValue, mainAreaMovementDuration));
            seq.Join(bottomArea.transform.DOMoveY(bottomArea.transform.position.y + mainAreaDeflectionValue, mainAreaMovementDuration));
        }
        public void OnClickBackButton() //Geri tu�una bast���m�zda ger�ekle�ecek i�lemler.
        {

            if (isPotionCrafted) //Craft i�lemini tamamlamadan geri d�n�lmesin.
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
                }); //Alt ve �st b�lgeyi ilgili y�ne do�ru hareket ettir, bu s�rada canvastaki t�m text ve image bile�enlerinin opasitesini d���r. ��lem bitti�inde ilgili screeni y�kle.
                bottomArea.DOMoveY(bottomArea.position.y - mainAreaDeflectionValue, mainAreaMovementDuration);    
            }
        }

        private async void OpenMenuScreen() //Ana men� screenini y�kledi�imiz fonksiyon.
        {
            var screenManager = ScreenManager.Instance;
            await screenManager.OpenScreen(ScreenKeys.MenuScreen, ScreenLayerKeys.FirstLayer);
        }

        IEnumerator ButtonTextTypeWriteEffect() //Craft s�ras�nda butonda yazacak yaz�y� daktilo efekti ile yazd�rmam�z� sa�layan coroutine.
        {
            foreach (char item in buttonText)
            {
                disableInCraftingButtonText.text += item.ToString();
                yield return new WaitForSeconds((lineFillingDuration * .5f) / buttonText.Length);
            }
        }
        IEnumerator RecipeTypeWriteEffect() //�ksire t�klad���m�zda tarif k�sm�nda yazacak yaz�y� daktilo efekti ile yazd�rmam�z� sa�layan coroutine.
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

            craftButton.interactable = true; //Yazma i�lemi bittikten sonra craf butonuna t�klayabilirli�i a�.
        }


    }
}
