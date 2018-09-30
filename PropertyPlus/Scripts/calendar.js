cal_days_labels = ['SU', 'MO', 'TU', 'WE', 'TH', 'FR', 'SA'];
cal_months_labels = ['January', 'February', 'March', 'April',
                 'May', 'June', 'July', 'August', 'September',
                 'October', 'November', 'December'];
// these are the days of the week for each month, in order
cal_days_in_month = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

// this is the current date
cal_current_date = new Date();

function Calendar(month, year) {
    this.month = (isNaN(month) || month == null) ? cal_current_date.getMonth() : month;
    this.year = (isNaN(year) || year == null) ? cal_current_date.getFullYear() : year;
    this.html = '';
}


Calendar.prototype.generateHTML = function (startAt,endAt) {
	var startDay = new Date(startAt.getFullYear(),startAt.getMonth(),startAt.getDate());
	var endDay = new Date(endAt.getFullYear(),endAt.getMonth(),endAt.getDate());
	console.log(startDay);
	console.log(endDay);
	var currentDay;
    // get first day of month
    var firstDay = new Date((new Date().getFullYear(), 0, 1));
    //var firstDay = new Date(this.year, this.month, 1);
    var startingDay = firstDay.getDay();

    // find number of days in month
    var monthLength = cal_days_in_month[this.month];

    // compensate for leap year
    if (this.month == 1) { // February only!
        if ((this.year % 4 == 0 && this.year % 100 != 0) || this.year % 400 == 0) {
            monthLength = 29;
        }
    }

    // do the header
    var html = ""
    for(var cal=0; cal<1; cal++){
     var curr = new Date(this.year, (this.month+cal), 1);
     var startingDay = curr.getDay();
      var monthName = cal_months_labels[this.month+cal];
      var monthLength = cal_days_in_month[this.month+cal];
      html += '<table class="calendar-table">';
      html += '<tr class="calendar-header">';
      for (var i = 0; i <= 6; i++) {
          html += '<td class="calendar-header-day">';
          html += cal_days_labels[i];
          html += '</td>';
      }
      html += '</tr><tr>';

      // fill in the days
      var day = 1;
      // this loop is for is weeks (rows)
      for (var i = 0; i < 6; i++) {
          // this loop is for weekdays (cells)
          for (var j = 0; j <= 6; j++) {
          		currentDay = new Date(this.year,this.month,day);
          		if((day <= monthLength && (i > 0 || j >= startingDay))&&(currentDay >= startDay && currentDay <=endDay)){
          			html += '<td class="calendar-day active">';
          		}else{
          			html += '<td class="calendar-day">';
          		};
              if (day <= monthLength && (i > 0 || j >= startingDay)) {

                  html += day;
                  day++;
              }
              html += '</td>';
          }
          // stop making rows if we've run out of days
          if (day > monthLength) {
              break;
          } else {
              html += '</tr><tr>';
          }
      }
      html += '</tr></table>';
    
    }//end of calendar loop

    this.html = html;
    
}

Calendar.prototype.getHTML = function () {
    return this.html;
}

function rowMonth(startAt,endAt,year){
	var row1 = '<tr>';
	var row2 = '<tr>';
	var time,startTime,endTime;
	for (var i = 1; i <= 6; i++) {
		time = new Date(year,i-1);
		startTime = new Date(startAt.getFullYear(),startAt.getMonth());
		endTime = new Date(endAt.getFullYear(),endAt.getMonth());
		if(time >= startTime && time <= endTime){
			row1+='<td class="month-select active">'+i+'</td>';
		}else{
			row1+='<td class="month-select">'+i+'</td>';
		}
	}
	for (var i = 7; i <= 12; i++) {
		time = new Date(year,i-1);
		startTime = new Date(startAt.getFullYear(),startAt.getMonth());
		endTime = new Date(endAt.getFullYear(),endAt.getMonth());
		if(time >= startTime && time <= endTime){
			row2+='<td class="month-select active">'+i+'</td>';
		}else{
			row2+='<td class="month-select">'+i+'</td>';
		}
	}
	row1+='</tr>';
	row2+='</tr>';
	return row1+row2;

}
function generateTable(tableNumber,tables){
	var count = 0;
	var content = '';
	for (tableNumber; tableNumber < tables.length; tableNumber++) {
	   		content+=tables[tableNumber];
	   		count++;
	   		if (tables.length >= 2) {
	   			if(count==2){
		   			$('#table-calendar').html(content);
		   			break;
		   		}
	   		}else{
	   			$('#table-calendar').html(content);
	   			break;
	   		}
	   		
	   }
}
function generateCalendar(startAt, endAt) {
    var tables = [];
    
    for (var i = startAt.getFullYear(); i <= endAt.getFullYear(); i++) {
    	var table = '';
    	table += '<table class="table-month">'+
			'<tr>'+
				'<th class="year-select" colspan="6">'+i+'</th>'+
			'</tr>'+ 
			rowMonth(startAt,endAt,i)+
		'</table>';
		tables.push(table);
    }
	   
	   var tableNumber = 0;
	   generateTable(tableNumber,tables);
	   $('#prev-year').click(function(){
	   		if (tableNumber != 0) {
	   			console.log(tableNumber);
	   			tableNumber --;
	   			generateTable(tableNumber,tables);
	   		}
	   });
	   $('#next-year').click(function(){
	   	console.log(tables.length);
	   		if (tableNumber < tables.length-2) {
	   			console.log(tableNumber);
	   			tableNumber ++;
	   			generateTable(tableNumber,tables);
	   		}
	   })
	   $('#mainCalendar').on('click','.table-month .month-select',function(){
	   	console.log(startAt,endAt);
		var monthSelect = Number($(this).text()) - 1;
		var yearSelect = Number($(this).parent().parent().find('.year-select').text());
		console.log(monthSelect,yearSelect);
		var cal = new Calendar(monthSelect,yearSelect);
		cal.generateHTML(startAt,endAt);
		document.getElementById("calendar").innerHTML = cal.getHTML();
	});
}