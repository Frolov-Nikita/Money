using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Money.ViewModel.Converters
{
    public class EnumToItemsSource : MarkupExtension
    {
        private readonly Type _type;

        public EnumToItemsSource(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _type.GetEnumValues();//.GetMembers().SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>()).Select(x => x.Description).ToList();
            
        }
    }
}
