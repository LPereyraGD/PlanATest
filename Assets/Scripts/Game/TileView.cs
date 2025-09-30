using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Managers
{
    public class TileView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image tileImage;
        [SerializeField] private Button tileButton;

        private int x;
        private int y;
        private GridView gridView;
        private bool isInitialized;

        public int X => x;
        public int Y => y;

        private void Awake()
        {
            if (tileImage == null)
                tileImage = GetComponent<Image>();
            
            if (tileButton == null)
                tileButton = GetComponent<Button>();
        }

        public void Initialize(int x, int y, GridView presenter)
        {
            this.x = x;
            this.y = y;
            gridView = presenter;
            isInitialized = true;
        }

        public void UpdateVisual(Sprite spriteColor, bool isValid)
        {
            if (tileImage == null) return;

            if (!isValid)
            {
                tileImage.color = Color.red;
            }
            else
            {
                tileImage.sprite = spriteColor;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isInitialized || gridView == null)
                return;

            gridView.OnTileClicked(x, y);
        }

        private void OnValidate()
        {
            if (tileImage == null)
                tileImage = GetComponent<Image>();
            
            if (tileButton == null)
                tileButton = GetComponent<Button>();
        }
    }
}