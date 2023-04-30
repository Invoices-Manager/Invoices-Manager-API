namespace Invoices_Manager_API.Enums
{
    public enum ImportanceStateEnum
    {
        VeryImportant,
        Important,
        Neutral,
        Unimportant
    }

    public class ImportanceState
    {
        public static object[] GetEnums()
        {
            Type importanceStateType = typeof(ImportanceStateEnum);
            Array importanceStateValues = Enum.GetValues(importanceStateType);
            object[] importanceStates = new object[importanceStateValues.Length];

            for (int i = 0; i < importanceStateValues.Length; i++)
            {
#pragma warning disable CS8605 // Unboxing eines möglichen NULL-Werts.
                ImportanceStateEnum importanceState = (ImportanceStateEnum)importanceStateValues.GetValue(i);
#pragma warning restore CS8605 // Unboxing eines möglichen NULL-Werts.
                importanceStates[i] = new { Name = importanceState.ToString(), Id = (int)importanceState };
            }

            return importanceStates;
        }
    }
}
