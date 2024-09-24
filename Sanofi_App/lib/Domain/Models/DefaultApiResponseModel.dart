import 'dart:convert';

class DefaultApiResponseModel{
  final String? message;
  final bool success;
  DefaultApiResponseModel({required this.success, this.message});

  static DefaultApiResponseModel fromJson(Map<String, dynamic> json){
    return DefaultApiResponseModel(
      message: json['message'],
      success: json['success']
    );
  }

  String toJson(){
    return jsonEncode(this);
  }
}