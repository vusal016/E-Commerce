namespace Engagement.Domain.SupportAggregate
{
    public sealed class Faq : AuditEntity
    {
        private Faq()
        {
        }
        public Faq(string category, string question, string answer, int orderNumber)
        {
            SetCategory(category);
            SetQuestion(question);
            SetAnswer(answer);
            SetOrderNumber(orderNumber);
        }
        public string Category { get; private set; }
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public int OrderNumber { get; private set; }
        private void SetCategory(string category)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(category, "Category cannot be empty.");
            Category = category;
        }
        private void SetQuestion(string question)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(question, "Question cannot be empty.");
            Question = question;
        }
        private void SetAnswer(string answer)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(answer, "Answer cannot be empty.");
            Answer = answer;
        }
        private void SetOrderNumber(int orderNumber)
        {
            if (orderNumber < 0)
                throw new ArgumentException("Order number cannot be negative.");
            OrderNumber = orderNumber;
        }
    }
}