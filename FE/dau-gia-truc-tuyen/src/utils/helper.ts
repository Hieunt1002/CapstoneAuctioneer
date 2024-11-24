export const convertDate = (startTime: string, endDate: string) => {
    const [day, month, year] = endDate?.split('/') || '0'; 
    const formattedDate = `${year}-${month}-${day}`;
    const isoDateString = `${formattedDate}T${startTime}:00`;
    const targetDate = new Date(isoDateString);
    return targetDate;
}

export const parseDateTime = (dateTime: string = '00/00/0000 00:00'): Date => {
  if (!dateTime || typeof dateTime !== 'string' || !dateTime.includes(" ")) {
    // Nếu dateTime không hợp lệ, trả về một Date mặc định hoặc xử lý theo cách khác
    console.error("Invalid dateTime format:", dateTime);
    return new Date(); // Hoặc trả về một Date mặc định khác
  }

  const [date, time] = dateTime.split(" ");
  const [day, month, year] = date.split("/").map(Number);
  const [hours, minutes] = time.split(":").map(Number);

  // Kiểm tra nếu các giá trị đều hợp lệ (chắc chắn là số)
  if (isNaN(day) || isNaN(month) || isNaN(year) || isNaN(hours) || isNaN(minutes)) {
    console.error("Invalid dateTime values:", dateTime);
    return new Date(); // Hoặc trả về một Date mặc định khác
  }

  return new Date(year, month - 1, day, hours, minutes);
};