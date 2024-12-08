import { Box, Button, Grid, MenuItem, TextField, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { getCategory } from '../../queries/index';
import ContractModal, { AuctionItemFormData } from '../modal-contract/ContractModal';
import avt from '../../../public/2937095.png';

const AuctionItemForm: React.FC = () => {
  const [listCategory, setCategory] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  // Fetch categories
  const fetchListCategory = async () => {
    try {
      const response = await getCategory();
      if (response?.isSucceed) {
        setCategory(response?.result);
      } else {
        console.error('Failed to fetch categories');
      }
    } catch (error) {
      console.error('Error fetching categories:', error);
    } finally {
      setLoading(false);
    }
  };

  const {
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<AuctionItemFormData>();
  const [previewImageAuction, setPreviewImageAuction] = useState('');
  const [previewImageVerification, setPreviewImageVerification] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [formData, setFormData] = useState<AuctionItemFormData | null>(null);

  const handleImageClick = (field: any, setFieldValue: any) => {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'image/*';
    input.onchange = (e: any) => {
      const files = e.target.files;
      if (files && files[0]) {
        const reader = new FileReader();
        reader.onload = (event: any) => {
          setFieldValue(event.target.result);
          field.onChange(files[0]);
        };
        reader.readAsDataURL(files[0]);
      }
    };
    input.click();
  };

  useEffect(() => {
    fetchListCategory();
  }, []);

  const onSubmits = (data: AuctionItemFormData) => {
    setFormData(data);
    setIsModalOpen(true);
  };

  return (
    <Box
      sx={{
        p: 4,
        height: '100%',
        backgroundColor: '#f9f9f9',
        borderRadius: 2,
        boxShadow: 3,
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}
    >
      <Box sx={{ maxWidth: 1200, width: '100%' }}>
        <Typography
          variant="h6"
          gutterBottom
          sx={{
            fontWeight: 'bold',
            color: '#3f51b5',
            textAlign: 'center',
            paddingBottom: '10px',
            marginTop: '50px',
            marginBottom: '50px',
          }}
        >
          Create Auction Item
        </Typography>
        <Box component="form" onSubmit={handleSubmit(onSubmits)} noValidate autoComplete="off">
          <Grid container spacing={4}>
            {/* Bên trái - Input Fields */}
            <Grid item xs={6}>
              <Grid container spacing={3}>
                {/* Name Auction */}
                <Grid item xs={12}>
                  <Controller
                    name="nameAuction"
                    control={control}
                    rules={{ required: 'Name is required' }}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        label="Name Auctioneer"
                        fullWidth
                        error={!!errors.nameAuction}
                        helperText={errors.nameAuction?.message}
                        sx={{
                          borderRadius: 1,
                          boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                        }}
                      />
                    )}
                  />
                </Grid>

                {/* Description */}
                <Grid item xs={12}>
                  <Controller
                    name="description"
                    control={control}
                    rules={{ required: 'Description is required' }}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        label="Description"
                        multiline
                        rows={4}
                        fullWidth
                        error={!!errors.description}
                        helperText={errors.description?.message}
                        sx={{
                          borderRadius: 1,
                          boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                        }}
                      />
                    )}
                  />
                </Grid>

                {/* Starting Price */}
                <Grid item xs={12} sm={6}>
                  <Controller
                    name="startingPrice"
                    control={control}
                    rules={{
                      required: 'Starting Price is required',
                      min: { value: 20000, message: 'Starting Price must be at least 20.000 VNĐ' },
                    }}
                    render={({ field }) => {
                      const handleChange = (event: any) => {
                        const rawValue = event.target.value.replace(/[^\d]/g, ''); // Loại bỏ ký tự không phải số
                        field.onChange(parseInt(rawValue || '0', 10)); // Gửi giá trị số nguyên lên server
                      };

                      const formattedValue = new Intl.NumberFormat('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      }).format(field.value || 0); // Hiển thị giá trị dạng tiền tệ

                      return (
                        <TextField
                          {...field}
                          value={formattedValue} // Hiển thị giá trị đã định dạng
                          onChange={handleChange} // Xử lý thay đổi
                          label="Starting Price"
                          type="text" // Dùng text để xử lý định dạng tiền tệ
                          fullWidth
                          error={!!errors.startingPrice}
                          helperText={errors.startingPrice?.message}
                          sx={{
                            borderRadius: 1,
                            boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                          }}
                        />
                      );
                    }}
                  />
                </Grid>

                {/* Category */}
                <Grid item xs={12} sm={6}>
                  <Controller
                    name="categoryID"
                    control={control}
                    rules={{ required: 'Category is required' }}
                    render={({ field }) => (
                      <TextField
                        {...field}
                        label="Category"
                        select
                        fullWidth
                        error={!!errors.categoryID}
                        helperText={errors.categoryID?.message}
                        disabled={loading}
                        sx={{
                          borderRadius: 1,
                          boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                        }}
                      >
                        {listCategory.map((category) => (
                          <MenuItem key={category.categoryID} value={category.categoryID}>
                            {category.nameCategory}
                          </MenuItem>
                        ))}
                      </TextField>
                    )}
                  />
                </Grid>
              </Grid>
            </Grid>
            {/* Bên phải - Upload Hình ảnh */}
            <Grid item xs={6}>
              <Grid container spacing={3}>
                {/* Image Auction */}
                <Grid item xs={6} sx={{ padding: '10px' }}>
                  <Typography
                    variant="subtitle1"
                    gutterBottom
                    sx={{ fontSize: 12, fontWeight: 'bold' }}
                  >
                    Image Auction
                  </Typography>
                  <Controller
                    name="imageAuction"
                    control={control}
                    rules={{ required: 'Image is required' }}
                    render={({ field }) => (
                      <div
                        onClick={() => handleImageClick(field, setPreviewImageAuction)}
                        className="w-[280px] h-[200px] cursor-pointer border border-gray-300 rounded flex items-center justify-center bg-gray-100"
                      >
                        <img
                          style={{ height: '200px' }}
                          src={previewImageAuction ? previewImageAuction : avt}
                          alt="Image Auction Preview"
                          className="w-full h-full object-cover rounded"
                        />
                      </div>
                    )}
                  />
                </Grid>

                {/* Image Verification */}
                <Grid item xs={6}>
                  <Typography
                    variant="subtitle1"
                    gutterBottom
                    sx={{ fontSize: 12, fontWeight: 'bold' }}
                  >
                    Image Verification
                  </Typography>
                  <Controller
                    name="imageVerification"
                    control={control}
                    render={({ field }) => (
                      <div
                        onClick={() => handleImageClick(field, setPreviewImageVerification)}
                        className="w-[280px] h-[200px] cursor-pointer border border-gray-300 rounded flex items-center justify-center bg-gray-100"
                      >
                        <img
                          style={{ height: '200px' }}
                          src={previewImageVerification ? previewImageVerification : avt}
                          alt="Image Verification Preview"
                          className="w-full h-full object-cover rounded"
                        />
                      </div>
                    )}
                  />
                </Grid>
              </Grid>
            </Grid>
          </Grid>

          {/* Nút Submit */}
          <Grid container justifyContent="center" sx={{ marginTop: 10, marginBottom: 6 }}>
            <Button type="submit" variant="contained" color="primary">
              Create Auction Item
            </Button>
          </Grid>
        </Box>

        {formData && (
          <ContractModal
            isOpen={isModalOpen}
            onClose={() => setIsModalOpen(false)}
            formData={formData}
          />
        )}
      </Box>
    </Box>
  );
};

export default AuctionItemForm;
