class ApiResponse{
  final int statusCode;
  final String responseBody;
  final String? message;

  ApiResponse({required this.statusCode, required this.responseBody, this.message});
}