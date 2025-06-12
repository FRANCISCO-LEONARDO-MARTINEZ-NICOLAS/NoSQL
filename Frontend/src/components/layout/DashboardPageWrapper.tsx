import React from 'react';
import { DashboardLayout } from './DashboardLayout';

interface DashboardPageWrapperProps {
  children: React.ReactNode;
}

export function DashboardPageWrapper({ children }: DashboardPageWrapperProps) {
  return (
    <DashboardLayout>
      {children}
    </DashboardLayout>
  );
} 