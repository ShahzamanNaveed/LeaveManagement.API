using System.Text;

namespace LeaveManagement.API.Features.Email.Templates
{
    public static class LeaveSubmittedTemplate
    {
        public static string Build(
            string employeeName,
            string department,
            string leaveType,
            DateTime startDate,
            DateTime endDate,
            double numberOfDays,
            string reason)
        {

            string content =
                $"""
                <p>
                    A new leave request has been submitted and requires your review.
                </p>


                <h4>
                    Leave Details
                </h4>


                <table style="
                    width:100%;
                    border-collapse:collapse;
                    margin-top:15px;
                ">

                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Employee
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {employeeName}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Department
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {department}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Leave Type
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {leaveType}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Start Date
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {startDate:dd-MM-yyyy}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            End Date
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {endDate:dd-MM-yyyy}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Number Of Days
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {numberOfDays}
                        </td>
                    </tr>


                    <tr>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            Reason
                        </td>
                        <td style="padding:8px;border-bottom:1px solid #ddd;">
                            {reason}
                        </td>
                    </tr>


                </table>


                <p style="margin-top:20px;">
                    Please review this request from the Leave Management System.
                </p>
                """;


            return EmailTemplateBuilder.Build(
                "New Leave Request Submitted",
                content,
                "PENDING APPROVAL",
                "#f39c12");
        }
    }
}