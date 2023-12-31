using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    public class UITooltipManager : ScriptableObject
    {
        #region singleton
        private static UITooltipManager m_Instance;
        public static UITooltipManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = Resources.Load("TooltipManager") as UITooltipManager;

                return m_Instance;
            }
        }
        #endregion

        [SerializeField] private GameObject m_TooltipPrefab;

        /// <summary>
        /// Gets the tooltip prefab.
        /// </summary>
        public GameObject prefab { get {  return this.m_TooltipPrefab; } }

        [SerializeField] private int m_SpacerHeight = 6;

        /// <summary>
        /// Spacer height used for the spacer line.
        /// </summary>
        public int spacerHeight { get { return this.m_SpacerHeight; } }

        [SerializeField] private UITooltipLineStyle m_DefaultLineStyle = new UITooltipLineStyle();
        [SerializeField] private UITooltipLineStyle m_TitleLineStyle = new UITooltipLineStyle();
        [SerializeField] private UITooltipLineStyle m_AttributeLineStyle = new UITooltipLineStyle();
        [SerializeField] private UITooltipLineStyle m_DescriptionLineStyle = new UITooltipLineStyle();
        [SerializeField] private UITooltipLineStyle[] m_CustomStyles = new UITooltipLineStyle[0];

        /// <summary>
        /// Default line style used when no style is specified.
        /// </summary>
        public UITooltipLineStyle defaultLineStyle { get { return this.m_DefaultLineStyle; } }

        /// <summary>
        /// Title line style used for the tooltip title.
        /// </summary>
        public UITooltipLineStyle titleLineStyle { get { return this.m_TitleLineStyle; } }

        /// <summary>
        /// Attribute line style.
        /// </summary>
        public UITooltipLineStyle attributeLineStyle { get { return this.m_AttributeLineStyle; } }

        /// <summary>
        /// Description line style used for the description.
        /// </summary>
        public UITooltipLineStyle descriptionLineStyle { get { return this.m_DescriptionLineStyle; } }

        /// <summary>
        /// The custom styles array.
        /// </summary>
        public UITooltipLineStyle[] customStyles { get { return this.m_CustomStyles; } }

        /// <summary>
        /// Gets a custom style by the specified name.
        /// </summary>
        /// <param name="name">The custom style name.</param>
        /// <returns>The custom style or the default style if not found.</returns>
        public UITooltipLineStyle GetCustomStyle(string name)
        {
            if (this.m_CustomStyles.Length > 0)
            {
                foreach (UITooltipLineStyle style in this.m_CustomStyles)
                {
                    if (style.Name.Equals(name))
                        return style;
                }
            }

            return this.m_DefaultLineStyle;
        }
    }
}
