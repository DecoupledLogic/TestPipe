namespace TestPipe.Core.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	public interface IAction
	{
		Uri Url { get; }

		void AppendText(Func<IControl> element, string text);

		void AppendTextWithoutEvents(Func<IControl> element, string text);

		void Click(int x, int y);

		void Click(Func<IControl> element, int x, int y);

		void Click(Func<IControl> element);

		void DoubleClick(int x, int y);

		void DoubleClick(Func<IControl> element, int x, int y);

		void DoubleClick(Func<IControl> element);

		void DragAndDrop(int sourceX, int sourceY, int destinationX, int destinationY);

		void DragAndDrop(Func<IControl> source, Func<IControl> target);

		void EnterText(Func<IControl> element, string text);

		void EnterTextWithoutEvents(Func<IControl> element, string text);

		Func<IControl> Find(string selector);

		Func<IEnumerable<IControl>> FindMultiple(string selector);

		void Focus(Func<IControl> element);

		void Hover(int x, int y);

		void Hover(Func<IControl> element, int x, int y);

		void Hover(Func<IControl> element);

		void MultiSelectIndex(Func<IControl> element, int[] optionIndices);

		void MultiSelectText(Func<IControl> element, string[] optionTextCollection);

		void MultiSelectValue(Func<IControl> element, string[] optionValues);

		void Navigate(Uri url);

		void Press(string keys);

		void RightClick(Func<IControl> element);

		void SelectIndex(Func<IControl> element, int optionIndex);

		void SelectText(Func<IControl> element, string optionText);

		void SelectValue(Func<IControl> element, string optionValue);

		void TakeScreenshot(string screenshotName);

		void Type(string text);

		void UploadFile(Func<IControl> element, int x, int y, string fileName);

		void Wait();

		void Wait(int seconds);

		void Wait(TimeSpan timeSpan);

		void WaitUntil(Expression<Func<bool>> conditionFunc);

		void WaitUntil(Expression<Func<bool>> conditionFunc, TimeSpan timeout);

		void WaitUntil(Expression<Action> conditionAction);

		void WaitUntil(Expression<Action> conditionAction, TimeSpan timeout);
	}
}