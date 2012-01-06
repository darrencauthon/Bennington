using System;

namespace Bennington.EmailCommunication.TextProcessing
{
	public interface IProcessReplaceTagService
	{
		string FindAndReplaceTags(string textContainingReplaceTags, string tagToFind, string replaceValue);
	}

	public class ProcessReplaceTagService : IProcessReplaceTagService
	{
		public string FindAndReplaceTags(string textContainingReplaceTags, string tagToFind, string replaceValue)
		{
			if (textContainingReplaceTags == null) return string.Empty;

			tagToFind = string.Format("[:{0}:]", tagToFind);
			var workingCopyOfTextContainingReplaceTags = textContainingReplaceTags;
			
			var startIndex = 0;
			while ((startIndex = workingCopyOfTextContainingReplaceTags.IndexOf(tagToFind, startIndex, StringComparison.InvariantCultureIgnoreCase)) > -1)
			{
				var leftChunkOfWorkingCopy = workingCopyOfTextContainingReplaceTags.Substring(0, startIndex);
				var indexOfFirstCharacterAfterTag = startIndex + tagToFind.Length;
				var lengthFromAfterTagToEndOfWorkingCopy = workingCopyOfTextContainingReplaceTags.Length - indexOfFirstCharacterAfterTag;
				var rightChunkOfWorkingCopy = workingCopyOfTextContainingReplaceTags.Substring(indexOfFirstCharacterAfterTag, lengthFromAfterTagToEndOfWorkingCopy);

				workingCopyOfTextContainingReplaceTags = string.Format("{0}{1}{2}", leftChunkOfWorkingCopy, replaceValue, rightChunkOfWorkingCopy);
			}

			return workingCopyOfTextContainingReplaceTags;
		}

	}
}
