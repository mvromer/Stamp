grammar Stamp;

computedString: atom* EOF ;

atom: substituionExpression
    | content
    ;

substituionExpression: START_DELIM content? END_DELIM ;

content: ~(START_DELIM | END_DELIM)+ ;

START_DELIM: '{|' ;
END_DELIM: '|}' ;
ANY: . ;
