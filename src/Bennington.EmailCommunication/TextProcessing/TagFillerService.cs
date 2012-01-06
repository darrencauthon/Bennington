using System;
using System.Reflection;

namespace Bennington.EmailCommunication.TextProcessing
{
	public interface ITagFillerService
	{
		string AutoFillTagsFromModel(string textContainingReplaceTags, object model);
	}

	public class TagFillerService : ITagFillerService
	{
		private readonly IProcessReplaceTagService processReplaceTagService;

		public TagFillerService(IProcessReplaceTagService processReplaceTagService)
		{
			this.processReplaceTagService = processReplaceTagService;
		}

		public string AutoFillTagsFromModel(string textContainingReplaceTags, object model)
		{
			var workingText = textContainingReplaceTags;

			workingText = ProcessForModel(model, workingText);

			return workingText;
		}

		private string ProcessForModel(object model, string workingText)
		{
			var propertyInfos = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var propertyInfo in propertyInfos)
			{
				if ((propertyInfo.PropertyType == typeof(string))
						|| (propertyInfo.PropertyType == typeof(int)) || (propertyInfo.PropertyType == typeof(int?))
						|| (propertyInfo.PropertyType == typeof(decimal)) || (propertyInfo.PropertyType == typeof(decimal?))
						|| (propertyInfo.PropertyType == typeof(double)) || (propertyInfo.PropertyType == typeof(double?))
						|| (propertyInfo.PropertyType == typeof(DateTime)) || (propertyInfo.PropertyType == typeof(DateTime?))
						|| (propertyInfo.PropertyType == typeof(bool?)) || (propertyInfo.PropertyType == typeof(bool))
					)
				{
					var propertyValue = propertyInfo.GetValue(model, null);
					workingText = processReplaceTagService.FindAndReplaceTags(workingText, propertyInfo.Name, (propertyValue ?? string.Empty).ToString());
				} else
				{
					var propertyValue = propertyInfo.GetValue(model, null);
                    if (propertyInfo.PropertyType.IsArray) continue;
                    if (propertyInfo.PropertyType.Name.StartsWith("IEnumerable")) continue;
					if (propertyValue != null)
						workingText = ProcessForModel(propertyValue, workingText);
				}
			}
			return workingText;
		}
	}
}
