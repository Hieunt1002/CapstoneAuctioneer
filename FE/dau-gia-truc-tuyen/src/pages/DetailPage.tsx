// pages/DetailPage.tsx
import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { CarouselDetail, DetailInformation, DetailContent } from '@components/properties-detail';
import AuctionRoom from '@components/auction-room/AuctionRoom';
import { getDetailAuction, profileUser } from '../queries/index';
import { AuctionDetails, Account } from 'types';

const DetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const [auctionDetailInfor, setAuctionDetailInfor] = useState<AuctionDetails | null>(null);
  const [userProfile, setUserProfile] = useState<Account | null>(null);
  const currentPath = window?.location?.pathname;

  useEffect(() => {
    const fetchListAuction = async () => {
      const response = await getDetailAuction(id || '0');
      if (response?.isSucceed) {
        setAuctionDetailInfor(response?.result || null);
      } else {
        console.error('fetch list failed');
      }
    };
    fetchListAuction();
  }, [id]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await profileUser();
        setUserProfile(response.result);
      } catch (error) {}
    };
    fetchData();
  }, []);

  return (
   <div className="container mx-auto p-4 flex flex-col gap-6 mt-20">
    {auctionDetailInfor && (
      <div className="flex gap-6 max-w-1/2">
        <CarouselDetail imgList={auctionDetailInfor.image} />
        <div className="col-span-2 w-full">
          {currentPath.includes("phien-dau-gia") ? (
            <AuctionRoom auctionDetailInfor={auctionDetailInfor} />
          ) : (
            <DetailInformation auctionDetailInfor={auctionDetailInfor} />
          )}
        </div>
      </div>
    )}
    {auctionDetailInfor && <DetailContent auctionDetailInfor={auctionDetailInfor} />}
  </div>
  );
};

export default DetailPage;