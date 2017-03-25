grammar AendringDefinitionGrammar;

/*
 * Parser Rules
 */
 /*root definition */
aendringDefinition
	: phrase ('.'|':') EOF
	;

/* possible AD phrase types */
phrase
	:parentTargetChangedExp
	|parentTargetRemovedExp

	|insertLastExp
	|insertAfterExp
	|insertAfterChainExp
	|insertBeforeExp

	|removeExp
	|replaceExp
	|manualExp
	;

/* phrases */
parentTargetChangedExp
	: 'Paragraffen affattes s�ledes'
	;
parentTargetRemovedExp
	: 'Paragraffen udg�r'
	;
insertAfterChainExp
	:'I' elementChainExp ', '? 'inds�ttes efter' (quotedTextChangeExp|(lastElementExp ('som nyt stykke')))
	;
insertAfterExp:
    'Efter' (elementExp) ('inds�ttes som nyt stykke'|'inds�ttes som nye stykker'|'inds�ttes som ny paragraf'|'inds�ttes')
	;										  
	
insertLastExp
	: 'Som' lastElementExp 'inds�ttes'
	| 'I' elementChainExp ('inds�ttes som'|', inds�ttes som') lastElementExp ('og' INT)?
	;
insertBeforeExp
	: 'F�r' elementChainExp 'inds�ttes som nyt nummer'
	|'I' elementChainExp (', inds�ttes f�r'|'inds�ttes f�r') (elementOrOpregningExp ('som nye stykker'|'som nye numre'))
	;
removeExp
	: elementChainExp (', udg�r'|'udg�r'|', oph�ves'|'oph�ves')
	| 'I' elementChainExp 'udg�r' QUOTEDTEXT
	;
replaceExp
	: elementChainExp  (', affattes s�ledes'|'affattes s�ledes'|', oph�ves, og i stedet inds�ttes'|'oph�ves, og i stedet inds�ttes')
	| 'I' elementChainExp (', �ndres'|'�ndres') quotedTextChangeExp
	
	;

manualExp
	:'I'? elementChainExp (((', udg�r'|'udg�r') QUOTEDTEXT)|('oph�ves'|', oph�ves')) manuelTextBitExp+
	|elementChainExp ((', '|'og') ignoreableElementChainExp)+ manuelTextBitExp+
	;
manuelTextBitExp:
    ', ' ignoreableElementChainExp ('oph�ves'|', oph�ves')
	|(', og i' ignoreableElementChainExp (', udg�r'|'udg�r') QUOTEDTEXT)
	|(', og' ignoreableElementChainExp (', 'ignoreableElementChainExp)? ('oph�ves'|', oph�ves')) ', og i stedet inds�ttes'?
	|(', og i' ignoreableElementChainExp ('inds�ttes efter'|'inds�ttes efter') quotedTextChangeExp)
;


/*specialized meta element categories*/
elementOrOpregningExp
    :elementExp|opregningExp
	;
ignoreableElementChainExp
	:elementChainExp|multiElementExp
	;
multiElementExp:
    INT '. og' elementExp
	|elementChainExp ('og'|', og') elementChainExp
    ;
lastElementExp
	: elementExp|opregningExp
    ;


/* meta element categories */
elementChainExp
    : (elementExp) (', ' elementExp)* (', ' opregningExp)*
	|elementExp 'og' elementExp
	;

elementExp 
	: paragrafExp|stkExp|pktExp
	;
opregningExp
    :nummerOpregningExp
    ;
quotedTextChangeExp
	: QUOTEDTEXT 'til:' QUOTEDTEXT
	| QUOTEDTEXT ':' QUOTEDTEXT 
	;


/*concret element expressions*/

nummerOpregningExp
	: 'nr. ' INT
	;
paragrafExp 
	: '� ' INT LETTER?
	;
flereStkExp
	:
	'Stk. ' INT (('og'|'-'|',')INT)+
	;
stkExp 
	: ('Stk. '|'stk. ') INT
	;
pktExp
	: INT ('. pkt'|'. pkt.')
	;
//comma : ','|', '; 


/*
 * Lexer Rules
 */
INT : [0-9]+; 
LETTER : [a-z]|[A-Z]; 
QUOTEDTEXT : '�' FREETEXT '�';
fragment FREETEXT: .*?;
//SPACE:' ';
//ANYCHAR: .;

//WS	:	(' ' )-> channel(HIDDEN)
//	//| '\r' | '\n'
//	;