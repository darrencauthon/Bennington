using Bennington.Core.List;

namespace Bennington.Cms.List
{
    public class CmsListViewModelOptions : ListViewModelOptions
    {
        public CmsListViewModelOptions()
        {
            SearchAction = "Index";
            TitleViewName = "Title";
            ButtonsViewName = "Buttons";
            PagerViewName = "Pager";
            RowsViewName = "Rows";
            SearchFormViewName = "SearchForm";
            DefaultTitleViewName = "~/Views/ListManage/Title.cshtml";
            DefaultButtonsViewName = "~/Views/ListManage/Buttons.cshtml";
            DefaultPagerViewName = "~/Views/ListManage/Pager.cshtml";
            DefaultRowsViewName = "~/Views/ListManage/Rows.cshtml";
            DefaultSearchFormViewName = "~/Views/ListManage/SearchForm.cshtml";
        }
    }
}