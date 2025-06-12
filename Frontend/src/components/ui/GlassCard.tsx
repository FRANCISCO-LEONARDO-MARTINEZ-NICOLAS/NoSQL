import React from 'react';

interface GlassCardProps {
  children: React.ReactNode;
  className?: string;
  onClick?: () => void;
  hover?: boolean;
}

export function GlassCard({ children, className = '', onClick, hover = false }: GlassCardProps) {
  return (
    <div
      className={`
        backdrop-blur-lg bg-white/20 dark:bg-white/10 
        border border-white/30 dark:border-white/20
        rounded-2xl shadow-glass
        ${hover ? 'hover:bg-white/30 dark:hover:bg-white/20 hover:shadow-glass-lg transition-all duration-300' : ''}
        ${onClick ? 'cursor-pointer' : ''}
        ${className}
      `}
      onClick={onClick}
    >
      {children}
    </div>
  );
}