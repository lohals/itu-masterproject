grammar ParagrafIndledning;
options{
	backtrack=true;
}
/*
 * Parser Rules
 */

 /*root definition */
paragrafIndledning
	: dokumentPhraseType2
	|dokumentPhraseType1
	;



//I lov nr. 606 af 12. juni 2013 om offentlighed i forvaltningen foretages følgende ændring:
dokumentPhraseType2
	: 'I ' dokumentReference SPLIT_TITLE
	;

dokumentPhraseType1
	:TITLE 'jf.'? dokumentReference
	;


//lov nr. 463 af 17. juni 2008
dokumentReference
	: //'lov nr. 463 af 17. juni 2008'
	doctype ' nr.' INT 'af' date
    ;

date:
	day MONTH year
	;
day
	: INT '.'
	;
year
	:INT
	;
/*
 * Lexer Rules
 */


SPLIT_TITLE_START:'om';
SPLIT_TITLE_END:', som ændret ved' | 'foretages følgende ændring';
SPLIT_TITLE:  SPLIT_TITLE_START FREETEXT SPLIT_TITLE_END;

START_TITLE: 'I ';
END_TITLE: ', ';
TITLE: START_TITLE FREETEXT END_TITLE;

doctype: LAW|LAWBEKENDTGORELSE;

INT : [0-9]+;
LAW: 'lov';
LAWBEKENDTGORELSE:'lov''\u00AD'?'bekendt''\u00AD'?'gørelse';
MONTH: 'januar'|'februar'|'marts'|'april'|'maj'|'juni'|'juli'|'august'|'september'|'oktober'|'november'|'december';
LETTER : [a-z]|[A-Z]; 




fragment FREETEXT: .*?;

WS
	:	' ' -> channel(HIDDEN)
	;