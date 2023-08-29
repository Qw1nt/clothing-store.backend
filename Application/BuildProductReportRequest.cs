namespace Application;

public record BuildProductReportRequest(List<int> ProductsIds, DateTime StartDate, DateTime EndDate);
