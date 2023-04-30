namespace Invoices_Manager_API.Enums
{
    public enum MoneyStateEnum
    {
        Paid,
        Received,
        NoInvoice
    }

    public class MoneyState
    {
        public static object[] GetEnums()
        {
            Type moneyStateType = typeof(MoneyStateEnum);
            Array moneyStateValues = Enum.GetValues(moneyStateType);
            object[] moneyStates = new object[moneyStateValues.Length];

            for (int i = 0; i < moneyStateValues.Length; i++)
            {
#pragma warning disable CS8605 // Unboxing eines möglichen NULL-Werts.
                MoneyStateEnum moneyState = (MoneyStateEnum)moneyStateValues.GetValue(i);
#pragma warning restore CS8605 // Unboxing eines möglichen NULL-Werts.
                moneyStates[i] = new { Name = moneyState.ToString(), Id = (int)moneyState };
            }

            return moneyStates;
        }
    }
}
