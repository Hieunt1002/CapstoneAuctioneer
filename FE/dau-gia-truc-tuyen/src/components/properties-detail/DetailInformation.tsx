import CountdownTimer from '@common/coutdown-timer/CountdownTimer';
import { AuctionDetails } from 'types';
import { convertDate } from '@utils/helper';
import { useNavigate } from 'react-router-dom';

interface InfoRowProps {
  label: string;
  value: string;
}

interface DetailInformationProps {
  auctionDetailInfor: AuctionDetails; // Use shared Auction type
}
const InfoRow: React.FC<InfoRowProps> = ({ label, value }) => (
  <>
    <div className="flex justify-between py-1">
      <div className="font-bold">{label}</div>
      <div className="font-bold">{value}</div>
    </div>
    <div className="h-[2px] w-full bg-gray-200"></div>
  </>
);

const DetailInformation: React.FC<DetailInformationProps> = ({ auctionDetailInfor }) => {
  const targetDate = convertDate(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay);
  const navigate = useNavigate();
  const calculateNewEndTime = (endTime: string, timePerLap: string): string => {
    // Tách giờ và phút từ endTime
    const [endHours, endMinutes] = endTime.split(':').map(Number);
    // Tách giờ và phút từ timePerLap
    const [lapHours, lapMinutes] = timePerLap.split(':').map(Number);
    
    // Tạo đối tượng Date với giờ và phút từ endTime
    const endDate = new Date();
    endDate.setHours(endHours);
    endDate.setMinutes(endMinutes);
  
    // Cộng thêm giờ và phút từ timePerLap
    endDate.setHours(endDate.getHours() + lapHours);
    endDate.setMinutes(endDate.getMinutes() + lapMinutes);
  
    // Lấy giờ và phút sau khi cộng thêm
    const newHours = endDate.getHours().toString().padStart(2, '0');
    const newMinutes = endDate.getMinutes().toString().padStart(2, '0');
  
    // Trả về chuỗi thời gian mới
    return `${newHours}:${newMinutes}`;
  };

  const updateEndTimeAndDay = (
    endTime: string,
    endDay: string,
    timePerLap: string
  ): { newEndTime: string; newEndDay: string } => {
      const [endHours, endMinutes] = endTime.split(':').map(Number);
      const [lapHours, lapMinutes] = timePerLap.split(':').map(Number);
  
      const endDate = new Date(endDay); // Chuyển đổi endDay thành Date
      endDate.setHours(endHours, endMinutes, 0, 0);
  
      endDate.setHours(endDate.getHours() + lapHours);
      endDate.setMinutes(endDate.getMinutes() + lapMinutes);
  
      const newHours = endDate.getHours().toString().padStart(2, '0');
      const newMinutes = endDate.getMinutes().toString().padStart(2, '0');
      const newEndDay = endDate.toISOString().split('T')[0]; // Lấy ngày định dạng YYYY-MM-DD
  
      return { newEndTime: `${newHours}:${newMinutes}`, newEndDay };
  };
  
  const calculateFinalTime = (
    endTime: string,
    endDay: string
  ): Date => {
    const [endHours, endMinutes] = endTime.split(':').map(Number); // Tách giờ và phút từ endTime
  
    const endDate = new Date(endDay); // Tạo đối tượng Date từ endDay
    endDate.setHours(endHours, endMinutes, 0, 0); // Gán giờ và phút từ endTime
  
    return endDate; // Trả về đối tượng Date đã được tính toán
  };
  
  const isRegistrationAllowed = (
    endTime: string,
    endDay: string
  ): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);
    return finalTime > new Date(); // Kiểm tra nếu thời gian kết thúc lớn hơn hiện tại
  };
  
  const auctionInfo = [
    { label: 'Giá khởi điểm', value: `${auctionDetailInfor?.startingPrice} VNĐ` },
    { label: 'Bước giá', value: `${auctionDetailInfor.priceStep} VNĐ` },
    { label: 'Tiền đặt trước', value: `${auctionDetailInfor.moneyDeposit} VNĐ` },
    {
      label: 'Thời gian đăng kí tham gia',
      value: `Từ ${auctionDetailInfor.startTime} ${auctionDetailInfor.startDay} đến ${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}`,
    },
    { label: 'Thời gian bắt đầu đấu giá', value: `Từ ${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay} đến ${calculateNewEndTime(auctionDetailInfor.endTime, auctionDetailInfor.timePerLap)} ${auctionDetailInfor.endDay}`, },
    { label: 'Hình thức đấu giá trực tuyến', value: 'Trả giá không xác định vòng' },
    { label: 'Phương thức trả giá', value: auctionDetailInfor.paymentMethod },
  ];
  const handleNavigateToContract = () => {
    navigate('/contract', {
      state: {
        // Contract data
        companyName: 'Tên Công ty ABC',
        companyAddress: '123 Đường ABC, Thành phố XYZ',
        taxCode: '0123456789',
        representativeName: 'Nguyễn Văn A',
        sellerName: 'Trần Văn B',
        sellerID: '123456789',
        sellerAddress: '456 Đường DEF, Thành phố XYZ',
        productName: 'Sản phẩm XYZ',
        websiteURL: 'https://example.com',
        effectiveDate: '05/11/2024',
        auctionId: auctionDetailInfor.listAuctionID,

        // Auction data
        auctionInfo: [
          { label: 'Giá khởi điểm', value: `${auctionDetailInfor?.startingPrice} VNĐ` },
          { label: 'Bước giá', value: `${auctionDetailInfor?.priceStep} VNĐ` },
          { label: 'Tiền đặt trước', value: `${auctionDetailInfor?.moneyDeposit} VNĐ` },
          {
            label: 'Thời gian đăng kí tham gia',
            value: `Từ ${auctionDetailInfor?.startTime} ${auctionDetailInfor?.startDay} đến ${auctionDetailInfor?.endTime} ${auctionDetailInfor?.endDay}`,
          },
          { label: 'Thời gian bắt đầu đấu giá', value: '09:00:00 31/10/2024' },
          { label: 'Hình thức đấu giá trực tuyến', value: 'Trả giá không xác định vòng' },
          { label: 'Phương thức trả giá', value: auctionDetailInfor?.paymentMethod },
        ],
      },
    });
  };
  return (
    <div className="container flex flex-col gap-2 h-full">
      <div className="flex gap-1">
        <div className="font-bold line-clamp-2">{auctionDetailInfor?.description}</div>
        <div className=" bg-green-500 bg-opacity-90 p-2 rounded-full w-60">
          <CountdownTimer targetDate={targetDate} />
        </div>
      </div>

      <div className="h-[2px] w-full bg-gray-200"></div>

      <div className="bg-gray-100 w-full h-full p-8 flex flex-col gap-1">
        {auctionInfo.map((item, index) => (
          <InfoRow key={index} label={item.label} value={item.value} />
        ))}
        <div className="mt-8 ml-auto mr-auto">
          <button
            className="bg-blue-500 text-white px-2 py-1 rounded mr-2 w-56 h-10"
            onClick={handleNavigateToContract}
            disabled={!isRegistrationAllowed(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay)}
          >
            THAM GIA ĐẤU GIÁ
          </button>
        </div>
      </div>
    </div>
  );
};

export default DetailInformation;
