import 'dart:convert';
import 'package:http/http.dart' as http;
import '../../Domain/ApiResponseModel.dart';

Future<ApiResponse> apiRequest( String type,
                            String url,
                            Map<String, String> headers,
                            String? body,
                            Map<String,dynamic>? parameters) async{
  if (url.isEmpty){
    throw ArgumentError('Url de requisição não pode ser vazia|nula');
  }

  headers['Accept'] = 'application/json; charset=UTF-8';
  headers['Content-Type'] = 'application/json; charset=UTF-8';
  type = type.toUpperCase();

  if (!['GET','POST','PUT','PATCH','DELETE','HEAD','OPTIONS'].contains(type)){
    throw ArgumentError('Método de requisição inválido');
  }

  if (parameters != null && parameters.isNotEmpty){
    url += '?';
    for(var parameter in parameters.keys){
      url += '$parameter=${headers[parameter]}&';
    }
    url = url.substring(0, url.length - 1);
  }
  final request = http.Request(type, Uri.parse(url));
  if (body != null && body.isNotEmpty) {
    request.body = body;
  }
  request.headers.addAll(headers);
  final streamedResponse = await http.Client().send(request);
  final http.Response response = await http.Response.fromStream(streamedResponse);
  return ApiResponse(statusCode: response.statusCode, responseBody: utf8.decode(response.bodyBytes));
}