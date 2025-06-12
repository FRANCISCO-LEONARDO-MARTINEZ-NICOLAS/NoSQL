import React from 'react';
import { LucideIcon } from 'lucide-react';
import { GlassCard } from './GlassCard';

interface MetricCardProps {
  title: string;
  value: string | number;
  icon: LucideIcon;
  trend?: {
    value: number;
    isPositive: boolean;
  };
  color?: 'primary' | 'secondary' | 'accent' | 'neutral';
}

export function MetricCard({ title, value, icon: Icon, trend, color = 'primary' }: MetricCardProps) {
  const colors = {
    primary: {
      bg: 'bg-gradient-to-br from-primary-500 to-primary-600',
      icon: 'text-primary-100',
      text: 'text-primary-50'
    },
    secondary: {
      bg: 'bg-gradient-to-br from-secondary-500 to-secondary-600',
      icon: 'text-secondary-100',
      text: 'text-secondary-50'
    },
    accent: {
      bg: 'bg-gradient-to-br from-accent-400 to-accent-500',
      icon: 'text-accent-100',
      text: 'text-accent-50'
    },
    neutral: {
      bg: 'bg-gradient-to-br from-neutral-500 to-neutral-600',
      icon: 'text-neutral-100',
      text: 'text-neutral-50'
    }
  };

  const colorScheme = colors[color];

  return (
    <GlassCard className="p-6 relative overflow-hidden">
      {/* Background gradient */}
      <div className={`absolute inset-0 ${colorScheme.bg} opacity-10`} />
      
      <div className="relative flex items-center justify-between">
        <div className="flex-1">
          <p className="text-sm font-medium text-neutral-600 dark:text-neutral-400 mb-1">
            {title}
          </p>
          <p className="text-2xl font-bold font-montserrat text-neutral-900 dark:text-neutral-100">
            {value}
          </p>
          {trend && (
            <div className="flex items-center mt-2">
              <span 
                className={`text-sm font-medium ${
                  trend.isPositive ? 'text-secondary-600' : 'text-red-500'
                }`}
              >
                {trend.isPositive ? '+' : ''}{trend.value}%
              </span>
              <span className="text-xs text-neutral-500 ml-1">vs mes anterior</span>
            </div>
          )}
        </div>
        
        <div className={`p-3 rounded-xl ${colorScheme.bg}`}>
          <Icon className={`w-6 h-6 ${colorScheme.icon}`} />
        </div>
      </div>
    </GlassCard>
  );
}