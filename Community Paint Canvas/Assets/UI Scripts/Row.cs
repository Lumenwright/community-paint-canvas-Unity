using UnityEngine.Scripting;
using UnityEngine.UIElements;
using UnityEngine;


public class Row : VisualElement
{
    public Row(){
        AddToClassList("row-container");
        VisualTreeAsset _rowTemplate = Resources.Load<VisualTreeAsset>("Row_Template");
        _rowTemplate.CloneTree(this);
    }
}
     #region UXML
        [Preserve]
        public class RowFactory : UxmlFactory<Row, RowTraits> { }

        [Preserve]
        public class RowTraits : VisualElement.UxmlTraits { }
        #endregion
