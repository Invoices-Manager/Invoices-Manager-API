namespace Invoices_Manager_API.Enums
{
    public enum PaidStateEnum
    {
        Paid,
        Unpaid,
        NoInvoice
    }

    public class PaidState
    {
        public static object[] GetEnums()
        {
            Type paidStateType = typeof(PaidStateEnum);
            Array paidStateValues = Enum.GetValues(paidStateType);
            object[] paidStates = new object[paidStateValues.Length];

            for (int i = 0; i < paidStateValues.Length; i++)
            {
#pragma warning disable CS8605 // Unboxing eines möglichen NULL-Werts.
                PaidStateEnum paidState = (PaidStateEnum)paidStateValues.GetValue(i);
#pragma warning restore CS8605 // Unboxing eines möglichen NULL-Werts.
                paidStates[i] = new { Name = paidState.ToString(), Id = (int)paidState };
            }

            return paidStates;
        }
    }
}
