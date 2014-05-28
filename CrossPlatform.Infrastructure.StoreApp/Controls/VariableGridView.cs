using CrossPlatform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace CrossPlatform.Infrastructure.StoreApp.Controls
{
    /// <summary>
    /// 가변 그리드 뷰
    /// </summary>
    public class VariableGridView : GridView
    {
        public VariableGridView()
            : base()
        {
        }

        public string ItemRowSpanPropertyPath
        {
            get { return (string)GetValue(ItemRowSpanPropertyPathProperty); }
            set { SetValue(ItemRowSpanPropertyPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemRowSpanPropertyPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemRowSpanPropertyPathProperty =
            DependencyProperty.Register("ItemRowSpanPropertyPath", typeof(string), typeof(VariableGridView), new PropertyMetadata(string.Empty));


        public string ItemColSpanPropertyPath
        {
            get { return (string)GetValue(ItemColSpanPropertyPathProperty); }
            set { SetValue(ItemColSpanPropertyPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemColSpanPropertyPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemColSpanPropertyPathProperty =
            DependencyProperty.Register("ItemColSpanPropertyPath", typeof(string), typeof(VariableGridView), new PropertyMetadata(string.Empty));

        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            UIElement uiElement = element as UIElement;

            if (EtcUtility.Instance.IsExistProperty(element, this.ItemColSpanPropertyPath) == true)
            {
                Binding colBinding = new Binding();
                colBinding.Source = item;
                colBinding.Path = new PropertyPath(this.ItemColSpanPropertyPath);
                BindingOperations.SetBinding(uiElement, VariableSizedWrapGrid.ColumnSpanProperty, colBinding);
            }

            if (EtcUtility.Instance.IsExistProperty(element, this.ItemRowSpanPropertyPath) == true)
            {
                Binding rowBinding = new Binding();
                rowBinding.Source = item;
                rowBinding.Path = new PropertyPath(this.ItemRowSpanPropertyPath);
                BindingOperations.SetBinding(uiElement, VariableSizedWrapGrid.RowSpanProperty, rowBinding);
            }
        }

    }
}
