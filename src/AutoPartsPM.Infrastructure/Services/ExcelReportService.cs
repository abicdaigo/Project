using AutoPartsPM.Application.Common.Interfaces;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsPM.Infrastructure.Services;

public class ExcelReportService(IApplicationDbContext context) : IReportService
{
    public async Task<byte[]> GenerateProjectReportAsync(int projectId, CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
            .Include(p => p.Customer)
            .Include(p => p.Phases).ThenInclude(ph => ph.Deliverables)
            .Include(p => p.Milestones)
            .Include(p => p.Issues)
            .Include(p => p.Equipment)
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken)
            ?? throw new InvalidOperationException($"プロジェクト ID {projectId} が見つかりません");

        using var workbook = new XLWorkbook();

        // Sheet 1: プロジェクト基本情報
        var infoSheet = workbook.Worksheets.Add("基本情報");
        infoSheet.Cell(1, 1).Value = "プロジェクトコード";
        infoSheet.Cell(1, 2).Value = project.ProjectCode;
        infoSheet.Cell(2, 1).Value = "プロジェクト名";
        infoSheet.Cell(2, 2).Value = project.ProjectName;
        infoSheet.Cell(3, 1).Value = "顧客名";
        infoSheet.Cell(3, 2).Value = project.Customer.CustomerName;
        infoSheet.Cell(4, 1).Value = "品番";
        infoSheet.Cell(4, 2).Value = project.PartNumber;
        infoSheet.Cell(5, 1).Value = "SOP日";
        infoSheet.Cell(5, 2).Value = project.SOPDate?.ToString("yyyy/MM/dd") ?? "-";
        infoSheet.Cell(6, 1).Value = "プロジェクトマネージャー";
        infoSheet.Cell(6, 2).Value = project.ProjectManagerName ?? "-";
        infoSheet.Column(1).Width = 25;
        infoSheet.Column(2).Width = 40;

        // Sheet 2: フェーズ進捗
        var phaseSheet = workbook.Worksheets.Add("APQPフェーズ");
        phaseSheet.Cell(1, 1).Value = "フェーズ";
        phaseSheet.Cell(1, 2).Value = "ステータス";
        phaseSheet.Cell(1, 3).Value = "進捗率";
        phaseSheet.Cell(1, 4).Value = "予定開始日";
        phaseSheet.Cell(1, 5).Value = "予定終了日";
        var phaseRow = 2;
        foreach (var phase in project.Phases.OrderBy(ph => ph.Phase))
        {
            phaseSheet.Cell(phaseRow, 1).Value = phase.Phase.ToString();
            phaseSheet.Cell(phaseRow, 2).Value = phase.Status.ToString();
            phaseSheet.Cell(phaseRow, 3).Value = (double)phase.CompletionRate;
            phaseSheet.Cell(phaseRow, 4).Value = phase.PlannedStartDate?.ToString("yyyy/MM/dd") ?? "-";
            phaseSheet.Cell(phaseRow, 5).Value = phase.PlannedEndDate?.ToString("yyyy/MM/dd") ?? "-";
            phaseRow++;
        }

        // Sheet 3: マイルストーン
        var msSheet = workbook.Worksheets.Add("マイルストーン");
        msSheet.Cell(1, 1).Value = "名称";
        msSheet.Cell(1, 2).Value = "フェーズ";
        msSheet.Cell(1, 3).Value = "期限";
        msSheet.Cell(1, 4).Value = "ステータス";
        var msRow = 2;
        foreach (var ms in project.Milestones.OrderBy(m => m.DueDate))
        {
            msSheet.Cell(msRow, 1).Value = ms.Name;
            msSheet.Cell(msRow, 2).Value = ms.Phase.ToString();
            msSheet.Cell(msRow, 3).Value = ms.DueDate.ToString("yyyy/MM/dd");
            msSheet.Cell(msRow, 4).Value = ms.Status.ToString();
            msRow++;
        }

        // Sheet 4: 課題
        var issueSheet = workbook.Worksheets.Add("課題");
        issueSheet.Cell(1, 1).Value = "タイトル";
        issueSheet.Cell(1, 2).Value = "優先度";
        issueSheet.Cell(1, 3).Value = "ステータス";
        issueSheet.Cell(1, 4).Value = "担当者";
        issueSheet.Cell(1, 5).Value = "期限";
        var issueRow = 2;
        foreach (var issue in project.Issues.OrderByDescending(i => i.Priority))
        {
            issueSheet.Cell(issueRow, 1).Value = issue.Title;
            issueSheet.Cell(issueRow, 2).Value = issue.Priority.ToString();
            issueSheet.Cell(issueRow, 3).Value = issue.Status.ToString();
            issueSheet.Cell(issueRow, 4).Value = issue.AssigneeName ?? "-";
            issueSheet.Cell(issueRow, 5).Value = issue.DueDate?.ToString("yyyy/MM/dd") ?? "-";
            issueRow++;
        }

        // Sheet 5: 設備
        var eqSheet = workbook.Worksheets.Add("設備");
        eqSheet.Cell(1, 1).Value = "コード";
        eqSheet.Cell(1, 2).Value = "設備名";
        eqSheet.Cell(1, 3).Value = "種別";
        eqSheet.Cell(1, 4).Value = "ステータス";
        eqSheet.Cell(1, 5).Value = "納期";
        var eqRow = 2;
        foreach (var eq in project.Equipment.OrderBy(e => e.Name))
        {
            eqSheet.Cell(eqRow, 1).Value = eq.EquipmentCode;
            eqSheet.Cell(eqRow, 2).Value = eq.Name;
            eqSheet.Cell(eqRow, 3).Value = eq.Type.ToString();
            eqSheet.Cell(eqRow, 4).Value = eq.Status.ToString();
            eqSheet.Cell(eqRow, 5).Value = eq.DeliveryDate?.ToString("yyyy/MM/dd") ?? "-";
            eqRow++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
