// pages/DetailPage.tsx
import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { CarouselDetail, DetailInformation, DetailContent } from '@components/properties-detail';
import AuctionRoom from '@components/auction-room/AuctionRoom';
import { getDetailAuction } from '../queries/index';
import { AuctionDetails } from 'types';
import { useMessage } from '@contexts/MessageContext';

const DetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const [auctionDetailInfor, setAuctionDetailInfor] = useState<AuctionDetails | null>(null);
  const [ switchdetail, setSwitchdetail ] = useState(false);
  const { setErrorMessage } = useMessage();
  useEffect(() => {
    const fetchListAuction = async () => {
      const response = await getDetailAuction(id || '0');
      if (response?.isSucceed) {
        setAuctionDetailInfor(response?.result || null);
      } else {
        setErrorMessage('fetch list failed');
      }
    };
    fetchListAuction();
  }, [id]);

  return (
   <div className="container mx-auto p-4 flex flex-col gap-6 mt-20">
    {auctionDetailInfor && (
      <div className="flex gap-6 max-w-1/2">
        <CarouselDetail imgList={auctionDetailInfor.image} />
        <div className="col-span-2 w-full">
          {switchdetail ? (
            <AuctionRoom auctionDetailInfor={auctionDetailInfor} />
          ) : (
            <DetailInformation auctionDetailInfor={auctionDetailInfor} setSwitchdetail={setSwitchdetail}/>
          )}
        </div>
      </div>
    )}
    {auctionDetailInfor && <DetailContent auctionDetailInfor={auctionDetailInfor} />}
  </div>
  );
};

export default DetailPage;
