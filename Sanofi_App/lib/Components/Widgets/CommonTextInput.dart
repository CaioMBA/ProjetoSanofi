import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:sanofi_app/Domain/Settings/GlobalSchematics.dart';

class CommonTextInput extends StatefulWidget {
  final TextEditingController controller;
  final String? hintText;
  final String? labelText;
  bool? obscureText;
  final String? type;
  final String? inputType;
  final void Function(String)? onSubmitted;
  final double? width;
  final bool? isPassword;

  CommonTextInput(
      {super.key,
        required this.controller,
        this.hintText,
        this.labelText,
        this.obscureText = false,
        this.type = 'NEXT',
        this.inputType = 'TEXT',
        this.onSubmitted,
        this.width,
        this.isPassword = false});


  @override
  State<CommonTextInput> createState() => _CommonTextInputState();
}


class _CommonTextInputState extends State<CommonTextInput> {
  TextInputType? InputFormatType;

  @override
  void initState() {
    widget.obscureText = widget.isPassword! ? true : false;
  }

  @override
  Widget build(BuildContext context) {
    TextInputFormatter? Formatter =
      FilteringTextInputFormatter.deny(RegExp(r''));

    switch (widget.inputType) {
      case 'TEXT':
        InputFormatType = TextInputType.text;
        break;
      case 'NUMBER':
        InputFormatType = TextInputType.number;
        Formatter = FilteringTextInputFormatter.digitsOnly;
        break;
      case 'PHONE':
        InputFormatType = TextInputType.phone;
        break;
      case 'EMAIL':
        InputFormatType = TextInputType.emailAddress;
        break;
      case 'DATE':
        InputFormatType = TextInputType.datetime;
        break;
      default:
        InputFormatType = TextInputType.text;
        break;
    }

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 5.0),
      child: SizedBox(
        width: widget.width ?? MediaQuery.of(context).size.width * 0.8,
        child: TextField(
          autocorrect: true,
          textAlign: TextAlign.center,
          controller: widget.controller,
          obscureText: widget.obscureText!,
          keyboardType: InputFormatType,
          inputFormatters: [Formatter],
          textInputAction: widget.type == 'DONE'
              ? TextInputAction.done
              : TextInputAction.next,
          onSubmitted: widget.onSubmitted ?? (value) {},
          decoration: InputDecoration(
            labelText: widget.labelText,
            enabledBorder: const OutlineInputBorder(
              borderSide: BorderSide(color: Colors.black),
            ),
            focusedBorder: OutlineInputBorder(
              borderSide: BorderSide(color: GlobalSchematics().primaryColor)
            ),
            fillColor: GlobalSchematics().whiteColor,
            filled: true,
            hintText: widget.hintText,
            suffixIcon: widget.isPassword!
                ? IconButton(
              icon: Icon(
                  widget.obscureText!
                      ? Icons.visibility
                      : Icons.visibility_off,
                  color: Theme.of(context).primaryColorDark),
              onPressed: () {
                setState(() {
                  widget.obscureText = !widget.obscureText!;
                });
              },
            )
                : null,
            hintStyle: TextStyle(color: Colors.grey.shade400),
          ),
        ),
      ),
    );
  }
}
