// components/ModalComponents.tsx
import React from 'react';
import { Modal, Box, Typography, Button, TextField } from '@mui/material';

interface ModalProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  setPrice?: (price: number) => void;
  users?: any[];
  setHours?: (time: number) => void;
  setMinutes?: (time: number) => void;
}

// ApproveModal Component
export const ApproveModal: React.FC<ModalProps> = ({
  open,
  onClose,
  onConfirm,
  setPrice,
  setHours,
  setMinutes,
}) => {
  const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
  };

  
  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={style}>
        <Typography variant="h6" component="h2">
          Xác nhận duyệt đấu giá
        </Typography>
        <Typography sx={{ mt: 2 }}>Bạn có chắc chắn muốn duyệt buổi đấu giá này không?</Typography>
        {/* Vòng thời gian */}
        <Box sx={{ display: 'flex', gap: 2, marginTop: '20px' }}>
          {/* Nhập số giờ */}
          <TextField
            label="Số giờ"
            type="number"
            onChange={(e) => (setHours ? setHours(Number(e.target.value)) : null)}
            inputProps={{
              min: 0, // Không cho nhập số âm
              step: 1, // Tăng theo từng giờ
              max: 99,
            }}
            sx={{ flex: 1 }}
          />
          {/* Nhập số phút */}
          <TextField
            label="Số phút"
            type="number"
            onChange={(e) => (setMinutes ? setMinutes(Number(e.target.value)) : null)}
            inputProps={{
              min: 0,
              max: 59, // Giới hạn phút từ 0 đến 59
              step: 1,
            }}
            sx={{ flex: 1 }}
          />
        </Box>
        <Box mt={2} display="flex" justifyContent="space-between">
          <Button
            variant="contained"
            color="success"
            onClick={() => {
              onConfirm();
            }}
          >
            Duyệt
          </Button>
          <Button variant="outlined" color="error" onClick={onClose}>
            Hủy
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

// CancelModal Component
export const CancelModal: React.FC<ModalProps> = ({ open, onClose, onConfirm }) => {
  const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={style}>
        <Typography variant="h6" component="h2">
          Xác nhận hủy đấu giá
        </Typography>
        <Typography sx={{ mt: 2 }}>Bạn có chắc chắn muốn hủy buổi đấu giá này không?</Typography>
        <Box mt={2} display="flex" justifyContent="space-between">
          <Button variant="contained" color="error" onClick={onConfirm}>
            Hủy đấu giá
          </Button>
          <Button variant="outlined" onClick={onClose}>
            Đóng
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};
export const UserModal: React.FC<ModalProps> = ({ open, onClose, users }) => {
  const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 500,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
    maxHeight: '80vh', // Set max height for the modal
    overflowY: 'auto', // Enable vertical scroll
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={style}>
        <Typography variant="h6" component="h2">
          Danh sách người đăng ký
        </Typography>
        {users && users.length > 0 ? (
          <Box
            sx={{ maxHeight: 200, overflowY: 'auto' }}
            className="scrollbar-thin scrollbar-thumb-gray-300 scrollbar-track-gray-100"
          >
            {' '}
            {/* Adjust height to show 4 items */}
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead className="bg-gray-200 sticky top-0 z-10">
                <tr>
                  <th>Tên người dùng</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user, index) => (
                  <tr key={index}>
                    <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.userName}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </Box>
        ) : (
          <Typography>Không có người đăng ký.</Typography>
        )}
        <Box mt={2} display="flex" justifyContent="space-between">
          <Button variant="contained" color="primary" onClick={onClose}>
            Đóng
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};
