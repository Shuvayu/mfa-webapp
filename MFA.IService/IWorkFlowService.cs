namespace MFA.IService
{
    public interface IWorkFlowService
    {
        /// <summary>
        /// Gets the formated text corrosponding to the workflow
        /// </summary>
        /// <param name="faceName"></param>
        /// <returns></returns>
        string InitiateWorkflow(string faceName);
    }
}
